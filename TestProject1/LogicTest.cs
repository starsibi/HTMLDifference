using AngleSharpExample;
using static Program;

namespace TestProject1
{
    [TestFixture]
    public class LogicTest
    {
        [Test]
        public void Compare_CallOut_And_Find_Difference_As_Nothing()
        {
            /*No Difference in the test data*/
            var oldHTML = "<blockquote style=\"background:#ddf7ff;border-left:4px solid #006a8a;overflow:auto;\" data-block-id=\"746fe92d-c036-43f2-a598-f637f85d678a\" class=\"infoBox\" data-background=\"#ddf7ff\" data-border=\"#006a8a\"><div class=\"blockquote-title\"><p data-block-id=\"c545504d-d8ff-403e-a923-cf51d9271584\">Title</p></div><p data-block-id=\"f5e911a3-70f9-4e9d-a481-f86f08997c12\">Content</p></blockquote><p data-block-id=\"3573742b-91ed-42ae-a5af-912d4a46beaf\"></p>";
            var newHTML = "<blockquote style=\"background:#ddf7ff;border-left:4px solid #006a8a;overflow:auto;\" data-block-id=\"746fe92d-c036-43f2-a598-f637f85d678a\" class=\"infoBox\" data-background=\"#ddf7ff\" data-border=\"#006a8a\"><div class=\"blockquote-title\"><p data-block-id=\"c545504d-d8ff-403e-a923-cf51d9271584\">Title</p></div><p data-block-id=\"f5e911a3-70f9-4e9d-a481-f86f08997c12\">Content</p></blockquote><p data-block-id=\"3573742b-91ed-42ae-a5af-912d4a46beaf\"></p>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = oldHTML.Replace("\\\"", "\"");

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Inserted_Something()
        {
            /*Difference in the test data*/
            var oldHTML = "<div>Welcome</div>";
            var newHTML = "<div>Welcome<p>paragraph</p></div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<div>Welcome<ins><p>paragraph</p></ins></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Deleted_Something()
        {
            /*Difference in the test data*/
            var oldHTML = "<div>Welcome<p>paragraph</p></div>";
            var newHTML = "<div>Welcome</div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<div>Welcome<del><p>paragraph</p></del></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Updated_Text_Something()
        {
            /*Difference in the test data*/
            var oldHTML = "<div>Welcome<p>paragraph</p></div>";
            var newHTML = "<div>Welcome<p>new paragraph</p></div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);            
            var expectedResult = "<div>Welcome<p><del>paragraph</del><ins>new paragraph</ins></p></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Deleted_Text_Something()
        {
            /*Difference in the test data*/
            /*Text with different scenario*/
            var oldHTML = "<div>Welcome<p>paragraph</p></div>";
            var newHTML = "<div>Welcome<p>para</p></div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<div>Welcome<p><del>paragraph</del><ins>para</ins></p></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Added_Text_Something()
        {
            /*Difference in the test data*/
            /*Text with unexpected scenario*/
            var oldHTML = "<div>Welcome<p></p></div>";
            var newHTML = "<div>Welcome<p>para</p></div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<div>Welcome<p><ins>para</ins></p></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Removed_Text()
        {
            /*Difference in the test data*/
            /*Text with missing scenario*/
            var oldHTML = "<div>Welcome<p>para</p></div>";
            var newHTML = "<div>Welcome<p></p></div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<div>Welcome<p><del>para</del></p></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Updated_Something_With_Multiple_Changes()
        {
            /*Difference in the test data*/
            /*Text with different scenario*/
            var oldHTML = "<div>Welcome<p>paragraph</p></div>";
            var newHTML = "<div>Welcome<p>new paragraph</p><p>another paragraph</p></div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<div>Welcome<p><del>paragraph</del><ins>new paragraph</ins></p><ins><p>another paragraph</p></ins></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }
        

        [Test]
        public void Compare_Div_And_Find_Difference_As_Attribute_Missing_Scenario()
        {
            /*Difference in the test data*/
            /*Attribute with missing scenario*/
            var oldHTML = "<div class=\"test\">Welcome</div>";
            var newHTML = "<div>Welcome</div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<mod><div>Welcome</div></mod>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Attribute_Different_Scenario()
        {
            /*Difference in the test data*/
            /*Attribute with different scenario*/
            var oldHTML = "<div class=\"test\">Welcome</div>";
            var newHTML = "<div class=\"different\">Welcome</div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<mod><div class=\"different\">Welcome</div></mod>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Div_And_Find_Difference_As_Attribute_Unexpected_Scenario()
        {
            /*Difference in the test data*/
            /*Attribute with unexpected scenario*/
            var oldHTML = "<div>Welcome</div>";
            var newHTML = "<div class=\"different\">Welcome</div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<mod><div class=\"different\">Welcome</div></mod>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }



        [Test]
        public void Compare_Div_And_Find_Difference_As_Updated_Something_With_Multiple_Changes_And_With_Nested_Changes()
        {
            /*Difference in the test data*/
            var oldHTML = "<div>Welcome<p>paragraph</p></div>";
            var newHTML = "<div>Welcome<p>new paragraph</p><p>another paragraph</p><p>third paragraph</p></div>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult = "<div>Welcome<p><del>paragraph</del><ins>new paragraph</ins></p><ins><p>another paragraph</p></ins><ins><p>third paragraph</p></ins></div>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Compare_Divs_And_Find_Difference_The_Removed_Child()
        {
            var oldHTML = "<div class=\"parent\"><div class=\"child1\">child1</div><div class=\"child2\">child2</div><div class=\"child3\">child3</div></div>";
            var newHTML = "<div class=\"parent\"><div class=\"child1\">child1</div><div class=\"child3\">child3</div></div>";


            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);

            var expectedResult = "<div class=\"parent\"><div class=\"child1\">child1</div><del><div class=\"child2\">child2</div></del><div class=\"child3\">child3</div></div>";
            Assert.That(result, Is.EqualTo(expectedResult));

        }


        [Test]
        public void Compare_Div_And_Find_Difference_As_Different_Element()
        {
            /*Difference in the test data*/
            var oldHTML = "<div>Welcome</div>";
            var newHTML = "<p>Welcome</p>";

            var Difference = new Difference();
            var result = Difference.GetHtmlDifference(oldHTML, newHTML);
            var expectedResult ="<ins><p>Welcome</p></ins><del><div>Welcome</div></del>";

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
