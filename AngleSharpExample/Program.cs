using AngleSharp;
using AngleSharp.Diffing;
using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing.Strategies.AttributeStrategies;
using AngleSharp.Diffing.Strategies.TextNodeStrategies;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Text;

public static class Program
{
    static void Main(string[] args)
    {
        var controlHtml = "<div></div>";
        var testHtml = "<p></p>";
        HTMLDifference htmlDifference = new HTMLDifference();
        var result = htmlDifference.GetHtmlDifference(controlHtml, testHtml);
        Console.WriteLine(result);

    }

    public class HTMLDifference
    {
        public string GetHtmlDifference(string controlHtml, string testHtml)
        {
            var strategy = new DiffingStrategyPipeline();
            strategy.AddDefaultOptions();
            strategy.AddFilter((in AttributeComparisonSource source, FilterDecision currentDecision) =>
            {
                if (source.Attribute.Name.Equals("data-blockid", StringComparison.OrdinalIgnoreCase))
                    return FilterDecision.Exclude;
                return currentDecision;
            });

            // Create the differ and pass in the strategy
            var differ = new HtmlDiffer(strategy);

            // Use the differ to compare markup
            //var controlHtml = "<div class=\"parent\">\r\n  <div class=\"child1\">child1</div>\r\n  <div class=\"child2\">child2</div>\r\n  <div class=\"child3\">child3</div>\r\n</div>";
            //var testHtml = "<div class=\"parent\">\r\n  <div class=\"child1\">child1</div>\r\n  <div class=\"child3\">child3</div>\r\n</div>";

            var diffs = differ.Compare(controlHtml, testHtml);

            return GenerateDiffHtml(controlHtml, testHtml, diffs);
        }
    }


    static string GenerateDiffHtml(string controlHtml, string testHtml, IEnumerable<IDiff> diffs)
    {
        var parser = new HtmlParser();
        var controlDocument = parser.ParseDocument(controlHtml);
        var testDocument = parser.ParseDocument(testHtml);

        var result = new StringBuilder();
        var output = parser.ParseDocument(testHtml);


        foreach (var diff in diffs)
        {
            try
            {
                if (diff.Target == DiffTarget.Attribute)
                {
                    switch (diff.Result)
                    {
                        case DiffResult.Different:
                            var differentDiff = (AttrDiff)diff;
                            var attribute = differentDiff.Control.Attribute;                          
                            var diffAttribute = differentDiff.Test.ElementSource.Node;
                            var differentAttributeSelector = diffAttribute.GetCssSelectorPath();
                            if (diffAttribute != null)
                            {
                                //Create mod Element here
                                var modAttributeElement = output.CreateElement("mod");
                                modAttributeElement.InnerHtml = differentDiff.Test.ElementSource.Node.ToHtml();

                                output?.QuerySelector(diffAttribute.GetCssSelectorPath())?.ReplaceWith(modAttributeElement);
                            }
                            break;

                        case DiffResult.Missing:
                            var missingDiff = (MissingAttrDiff)diff;
                            //var missingAttribute = missingDiff.Control.Attribute;
                            var missingAttribute = missingDiff.Control.ElementSource.Node;
                            var missingAttributeElement = output?.QuerySelector(missingAttribute.GetCssSelectorPath());
                            if(missingAttributeElement != null)
                            {
                                var modifyElement = output.CreateElement("mod");
                                modifyElement.InnerHtml = missingAttributeElement.ToHtml();
                                output?.QuerySelector(missingAttribute.GetCssSelectorPath())?.ReplaceWith(modifyElement);
                            }                           
                            break;

                        case DiffResult.Unexpected:
                            var unexpectedDiff = (UnexpectedAttrDiff)diff;
                            var unexpectedAttribute = unexpectedDiff.Test.ElementSource.Node;

                            var unexpectedAttributeSelector = unexpectedAttribute.GetCssSelectorPath();
                            if (unexpectedAttribute != null)
                            {

                                //Create mod Element here
                                var modAttributeElement = output.CreateElement("mod");
                                modAttributeElement.InnerHtml = unexpectedDiff.Test.ElementSource.Node.ToHtml();

                                output?.QuerySelector(unexpectedAttribute.GetCssSelectorPath())?.ReplaceWith(modAttributeElement);
                            }
                            
                            break;
                    }

                }
                else if (diff.Target == DiffTarget.Element)
                {

                    switch (diff.Result)
                    {
                        case DiffResult.Different:
                            //Element gets changed in the new HTML
                            var differentDiff = (DiffBase<ComparisonSource>)diff;
                            var differentNode = differentDiff.Test.Node;
                            result.Append($"<ins>{differentNode.ToHtml()}</ins>");
                            break;

                        case DiffResult.Missing:
                            //Element gets removed from the new HTML
                            var missingDiff = (MissingDiffBase<ComparisonSource>)diff;
                            var missingNode = missingDiff.Control.Node;
                            var missingNodeSelector = missingNode.GetCssSelectorPath();
                            //Change the below logic
                            var delElement = output.CreateElement("del");
                            delElement.InnerHtml = missingNode.ToHtml();

                            var parentElement = output.QuerySelector(missingNode.ParentElement?.GetCssSelectorPath() ?? string.Empty);
                            if (parentElement != null)
                            {
                                parentElement.AppendChild(delElement);
                            }                            

                            break;

                        case DiffResult.Unexpected:
                            //New Element gets added in the new HTML
                            var unexpectedDiff = (UnexpectedDiffBase<ComparisonSource>)diff;
                            var newNode = unexpectedDiff.Test.Node;
                            var newNodeSelector = newNode.GetCssSelectorPath();
                            var insElement = output.CreateElement("ins");
                            insElement.InnerHtml = newNode.ToHtml();
                            output.QuerySelector(newNodeSelector)?.ReplaceWith(insElement);

                            break;

                    }
                }
                else if (diff.Target == DiffTarget.Text)
                {
                    switch (diff.Result)
                    {
                        case DiffResult.Different:
                            var differentDiff = (TextDiff)diff;
                            var diffNode = differentDiff.Test.Node;
                            var differentSelector = diffNode.GetCssSelectorPath();
                            if (diffNode.Parent != null)
                            {
                                var replaceElement = output.CreateElement(diffNode.Parent.NodeName);

                                //Create del Element here
                                var delElement = output.CreateElement("del");
                                delElement.InnerHtml = differentDiff.Control.Node.ToHtml();

                                //Create ins Element here
                                var insElement = output.CreateElement("ins");
                                insElement.InnerHtml = diffNode.ToHtml();
                                replaceElement.AppendChild(delElement);
                                replaceElement.AppendChild(insElement);

                                output?.QuerySelector(differentSelector)?.ReplaceWith(replaceElement);
                            }
                        
                            break;

                        case DiffResult.Missing:
                            var missingDiff = (MissingDiffBase<ComparisonSource>)diff;
                            var missingNode = missingDiff.Control.Node;
                            var missingSelector = missingNode.GetCssSelectorPath();
                            var removedNode = controlDocument.Body?.QuerySelector(missingSelector);
                            
                            var delMissingElement = output.CreateElement("del");
                            delMissingElement.InnerHtml = missingNode.ToHtml();

                            var parentElement = output.QuerySelector(missingNode.ParentElement?.GetCssSelectorPath() ?? string.Empty);
                            if (parentElement != null)
                            {
                                parentElement.AppendChild(delMissingElement);
                            }
                           
                            break;

                        case DiffResult.Unexpected:
                            var unexpectedDiff = (UnexpectedDiffBase<ComparisonSource>)diff;
                            var newTextNode = unexpectedDiff.Test.Node;
                            var newNodeSelector = newTextNode.GetCssSelectorPath();
                            var insTextElement = output.CreateElement("ins");
                            insTextElement.InnerHtml = newTextNode.ToHtml();
                            if (newTextNode.Parent != null)
                            {
                                var replaceNodeElement = output.CreateElement(newTextNode.Parent.NodeName);
                                replaceNodeElement.AppendChild(insTextElement);
                                output.QuerySelector(newNodeSelector)?.ReplaceWith(replaceNodeElement);
                            }
                            break;

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return output?.Body?.InnerHtml ?? string.Empty;
    }
    // Helper method to convert AngleSharp paths to valid CSS selectors
    static string ConvertPathToSelector(string path)
    {
        if (string.IsNullOrEmpty(path))
            return "";

        // Split the path into parts
        var parts = path.Split(ComparisonSource.PathSeparatorChar);
        var selector = new StringBuilder();

        for (int i = 0; i < parts.Length; i++)
        {

            if (string.IsNullOrEmpty(parts[i]))
                continue;

            // Extract element name and index
            var match = System.Text.RegularExpressions.Regex.Match(parts[i], @"([a-zA-Z0-9]+)\((\d+)\)");
            if (match.Success)
            {
                var element = match.Groups[1].Value;
                var index = int.Parse(match.Groups[2].Value);

                // Build the selector for this part
                selector.Append($"{element}:nth-of-type({index + 1})");

            }
            else
            {
                // If the format is different, just use the part as is
                selector.Append(parts[i]);
            }

            selector.Append(" ");
        }

        return selector.ToString().Trim();
    }
    public class HTMLDiff
    {
        public List<IDiff> GetHtmlDiffs(string oldHTML, string newHTML)
        {
            var diffs = DiffBuilder
                .Compare(oldHTML)
                .WithTest(newHTML)
                .WithOptions(option => option
                    .AddDefaultOptions()
                    .AddIgnoreAttributesElementSupport()
                    .AddFilter((in AttributeComparisonSource source, FilterDecision currentDecision) =>
                    {
                        if (source.Attribute.Name.Equals("data-blockid", StringComparison.OrdinalIgnoreCase))
                            return FilterDecision.Exclude;
                        return currentDecision;
                    })
                )
                .Build();

            return diffs.ToList();


        }

    }


    static string GetCssSelectorPath(this INode node)
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

    static string BuildElementSelector(IElement element)
    {
        var selector = element.TagName.ToLower();

        // Add ID if present
        /*if (!string.IsNullOrEmpty(element.Id))
            return $"{selector}#{element.Id}";*/

        // Add classes if present
      /*  var classes = string.Join(".", element.ClassList);
        if (!string.IsNullOrEmpty(classes))
            selector += $".{classes}";*/

        // Add index if needed
        var index = GetElementIndex(element);
        if (index > 0)
            selector += $":nth-of-type({index})";

        return selector;
    }
    static int GetElementIndex(IElement element)
    {
        if (element == null || element.ParentElement == null)
            return 0;

        var siblings = element.ParentElement.Children
            .Where(e => e.TagName == element.TagName)
            .ToList();

        return siblings.Count > 1 ? siblings.IndexOf(element) + 1 : 0;
    }
}