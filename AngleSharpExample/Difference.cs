
using AngleSharp;
using AngleSharp.Diffing;
using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Strategies;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Text;

namespace AngleSharpExample
{
    public class Difference
    {
        private readonly HtmlParser _parser;
        private readonly DiffingStrategyPipeline _strategy;

        public Difference()
        {
            _parser = new HtmlParser();
            _strategy = new DiffingStrategyPipeline();
            InitializeStrategy();
        }

        private void InitializeStrategy()
        {
            _strategy.AddDefaultOptions();
            _strategy.AddFilter((in AttributeComparisonSource source, FilterDecision currentDecision) =>
            {
                return new[] { "data-blockid", "type", "data-block-id", "data-display" }.Contains(source.Attribute.Name, StringComparer.OrdinalIgnoreCase)
                    ? FilterDecision.Exclude
                    : currentDecision;
                //return source.Attribute.Name.Equals("data-blockid", StringComparison.OrdinalIgnoreCase)
                //    ? FilterDecision.Exclude
                //    : currentDecision;
            });
        }

        public string GetHtmlDifference(string controlHtml, string testHtml)
        {
            try
            {
                var differ = new HtmlDiffer(_strategy);
                var diffs = differ.Compare(controlHtml, testHtml);
                return GenerateDiffHtml(controlHtml, testHtml, diffs);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error comparing HTML: {ex.Message}", ex);
            }
        }

        private string GenerateDiffHtml(string controlHtml, string testHtml, IEnumerable<IDiff> diffs)
        {
            var output = _parser.ParseDocument(testHtml);
            var controlDocument = _parser.ParseDocument(controlHtml);

            foreach (var diff in diffs)
            {
                try
                {
                    switch (diff.Target)
                    {
                        case DiffTarget.Attribute:
                            HandleAttributeDiff(diff, output);
                            break;

                        case DiffTarget.Element:
                            HandleElementDiff(diff, output);
                            break;

                        case DiffTarget.Text:
                            HandleTextDiff(diff, output, controlDocument);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing diff: {ex.Message}");
                }
            }

            return output?.Body?.InnerHtml ?? string.Empty;
        }

        private void HandleAttributeDiff(IDiff diff, IDocument output)
        {
            switch (diff.Result)
            {
                case DiffResult.Different:
                    var differentDiff = (AttrDiff)diff;
                    ReplaceWithModElement(output, differentDiff.Test.ElementSource.Node);
                    break;

                case DiffResult.Missing:
                    var missingDiff = (MissingAttrDiff)diff;
                    //Here we will get the control element but we need to replace the test element so quering it with the output document to add the mod element
                    var missingAttributeElement = output.QuerySelector(missingDiff.Control.ElementSource.Node.GetCssSelectorPath());
                    if (missingAttributeElement != null)
                    {
                        ReplaceWithModElement(output, missingAttributeElement);
                    }
                    break;

                case DiffResult.Unexpected:
                    var unexpectedDiff = (UnexpectedAttrDiff)diff;
                    ReplaceWithModElement(output, unexpectedDiff.Test.ElementSource.Node);
                    break;
            }
        }

        private void HandleElementDiff(IDiff diff, IDocument output)
        {
            switch (diff.Result)
            {
                case DiffResult.Different:
                    var differentDiff = (DiffBase<ComparisonSource>)diff;
                    var differentNode = differentDiff.Test.Node;
                    var insElement = CreateInsElement(output, differentNode);
                    ReplaceNode(output, differentNode, insElement);
                    break;

                case DiffResult.Missing:
                    var missingDiff = (MissingDiffBase<ComparisonSource>)diff;
                    AppendDeletedElement(output, missingDiff.Control.Node);
                    break;

                case DiffResult.Unexpected:
                    var unexpectedDiff = (UnexpectedDiffBase<ComparisonSource>)diff;
                    var newNode = unexpectedDiff.Test.Node;
                    var newInsElement = CreateInsElement(output, newNode);
                    ReplaceNode(output, newNode, newInsElement);
                    break;
            }
        }

        private void HandleTextDiff(IDiff diff, IDocument output, IDocument controlDocument)
        {
            switch (diff.Result)
            {
                case DiffResult.Different:
                    var textDiff = (TextDiff)diff;
                    HandleDifferentText(output, textDiff);
                    break;

                case DiffResult.Missing:
                    var missingDiff = (MissingDiffBase<ComparisonSource>)diff;
                    AppendDeletedText(output, missingDiff.Control.Node);
                    break;

                case DiffResult.Unexpected:
                    var unexpectedDiff = (UnexpectedDiffBase<ComparisonSource>)diff;
                    HandleUnexpectedText(output, unexpectedDiff.Test.Node);
                    break;
            }
        }

        private void HandleDifferentText(IDocument output, TextDiff diff)
        {
            var diffNode = diff.Test.Node;
            if (diffNode.Parent != null)
            {
                var replaceElement = output.CreateElement(diffNode.Parent.NodeName);
                var delElement = CreateDeletedElement(output, diff.Control.Node);
                var insElement = CreateInsElement(output, diffNode);

                replaceElement.AppendChild(delElement);
                replaceElement.AppendChild(insElement);

                ReplaceNode(output, diffNode, replaceElement);
            }
        }

        private void HandleUnexpectedText(IDocument output, INode newTextNode)
        {
            if (newTextNode.Parent != null)
            {
                var replaceElement = output.CreateElement(newTextNode.Parent.NodeName);
                var insElement = CreateInsElement(output, newTextNode);
                replaceElement.AppendChild(insElement);
                ReplaceNode(output, newTextNode, replaceElement);
            }
        }

        private void ReplaceWithModElement(IDocument output, INode node)
        {
            var modElement = output.CreateElement("mod");
            modElement.InnerHtml = node.ToHtml();
            output.QuerySelector(node.GetCssSelectorPath())?.ReplaceWith(modElement);
        }

        private IElement CreateInsElement(IDocument output, INode node)
        {
            var element = output.CreateElement("ins");
            element.InnerHtml = node.ToHtml();
            return element;
        }

        private IElement CreateDeletedElement(IDocument output, INode node)
        {
            var element = output.CreateElement("del");
            element.InnerHtml = node.ToHtml();
            return element;
        }

        private void AppendDeletedElement(IDocument output, INode node)
        {
            var delElement = CreateDeletedElement(output, node);
            var parentElement = output.QuerySelector(node.ParentElement?.GetCssSelectorPath() ?? string.Empty);
            parentElement?.AppendChild(delElement);
        }

        private void AppendDeletedText(IDocument output, INode node)
        {
            var delElement = CreateDeletedElement(output, node);
            var parentElement = output.QuerySelector(node.ParentElement?.GetCssSelectorPath() ?? string.Empty);
            parentElement?.AppendChild(delElement);
        }

        private void ReplaceNode(IDocument output, INode node, IElement replacement)
        {
            output.QuerySelector(node.GetCssSelectorPath())?.ReplaceWith(replacement);
        }
    }

    public static class NodeExtensions
    {
        public static string GetCssSelectorPath(this INode node)
        {
            if (node == null)
                return string.Empty;

            var path = new StringBuilder();
            var current = node;

            while (current != null)
            {
                if (current.NodeType == NodeType.Element)
                {
                    var element = current as IElement;
                    var selector = BuildElementSelector(element);

                    if (path.Length > 0)
                        path.Insert(0, " > ");

                    path.Insert(0, selector);
                }
                else if (current.NodeType == NodeType.Text)
                {
                    var textNode = current as IText;
                    var parentElement = textNode?.ParentElement;
                    if (parentElement != null)
                    {
                        var selector = BuildElementSelector(parentElement);

                        if (path.Length > 0)
                            path.Insert(0, " > ");

                        path.Insert(0, selector);
                    }
                    current = parentElement;
                }

                current = current?.ParentElement;
            }

            return path.ToString();
        }

        private static string BuildElementSelector(IElement element)
        {
            if (element == null) return string.Empty;

            var selector = element.TagName.ToLower();
            var index = GetElementIndex(element);

            return index > 0 ? $"*:nth-child({index}):where({selector}, del {selector}, ins {selector}, mod {selector})" : selector;
        }

        private static int GetElementIndex(IElement element)
        {
            if (element?.ParentElement == null) return 0;

            var siblings = element.ParentElement.Children
                .Where(e => e.TagName == element.TagName)
                .ToList();

            return siblings.Count > 1 ? siblings.IndexOf(element) + 1 : 0;
        }
    }


}
