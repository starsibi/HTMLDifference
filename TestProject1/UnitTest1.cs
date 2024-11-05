//using Microsoft.VisualStudio.TestPlatform.TestHost;
//using static Program;

//namespace TestProject1
//{
//    public class Tests
//    {

//        [Test]
//        public void Compare_CallOut_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<blockquote style=\"background:#ddf7ff;border-left:4px solid #006a8a;overflow:auto;\" data-block-id=\"746fe92d-c036-43f2-a598-f637f85d678a\" class=\"infoBox\" data-background=\"#ddf7ff\" data-border=\"#006a8a\"><div class=\"blockquote-title\"><p data-block-id=\"c545504d-d8ff-403e-a923-cf51d9271584\">Title</p></div><p data-block-id=\"f5e911a3-70f9-4e9d-a481-f86f08997c12\">Content</p></blockquote><p data-block-id=\"3573742b-91ed-42ae-a5af-912d4a46beaf\"></p>";
//            var newHTML = "<blockquote style=\"background:#ddf7ff;border-left:4px solid #006a8a;overflow:auto;\" data-block-id=\"746fe92d-c036-43f2-a598-f637f85d678a\" class=\"infoBox\" data-background=\"#ddf7ff\" data-border=\"#006a8a\"><div class=\"blockquote-title\"><p data-block-id=\"c545504d-d8ff-403e-a923-cf51d9271584\">Title</p></div><p data-block-id=\"f5e911a3-70f9-4e9d-a481-f86f08997c12\">Content</p></blockquote><p data-block-id=\"3573742b-91ed-42ae-a5af-912d4a46beaf\"></p>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_CallOut_And_Find_Difference_Count_As_Four()
//        {
//            /*4 attributes are changed in newHTML*/
//            var oldHTML = "<blockquote style=\"background:#ddf7ff;border-left:4px solid #006a8a;overflow:auto;\" data-block-id=\"746fe92d-c036-43f2-a598-f637f85d678a\" class=\"infoBox\" data-background=\"#ddf7ff\" data-border=\"#006a8a\"><div class=\"blockquote-title\"><p data-block-id=\"c545504d-d8ff-403e-a923-cf51d9271584\">Title</p></div><p data-block-id=\"f5e911a3-70f9-4e9d-a481-f86f08997c12\">Content</p></blockquote><p data-block-id=\"3573742b-91ed-42ae-a5af-912d4a46beaf\"></p>";
//            var newHTML = "<blockquote style=\"background:#fdf2ce;border-left:4px solid #7f6416;overflow:auto;\" data-block-id=\"746fe92d-c036-43f2-a598-f637f85d678a\" class=\"warningBox\" data-background=\"#fdf2ce\" data-border=\"#7f6416\"><div class=\"blockquote-title\"><p data-block-id=\"c545504d-d8ff-403e-a923-cf51d9271584\">Title</p></div><p data-block-id=\"f5e911a3-70f9-4e9d-a481-f86f08997c12\">Content</p></blockquote><p data-block-id=\"3573742b-91ed-42ae-a5af-912d4a46beaf\"></p>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(4));
//        }

//        [Test]
//        public void Compare_Table_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<table><tr><td>1</td><td>2</td></tr></table>";
//            var newHTML = "<table><tr><td>1</td><td>2</td></tr></table>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Table_And_Find_Difference_Count_As_Two()
//        {
//            /*2 Table data is changed in newHTML*/
//            var oldHTML = "<table><tr><td>1</td><td>2</td></tr></table>";
//            var newHTML = "<table><tr><td>2</td><td>3</td></tr></table>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(2));
//        }

//        [Test]
//        public void Compare_Image_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<img src=\"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png\" alt=\"Google\">";
//            var newHTML = "<img src=\"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png\" alt=\"Google\">";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Image_And_Find_Difference_Count_As_Two()
//        {
//            var oldHTML = "<img src=\"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png\" alt=\"Google\">";
            
//            /*Height and width attributes are added in newHTML*/
//            var newHTML = "<img src=\"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png\" alt=\"Google\" width=\"100\" height=\"100\">";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(2));
//        }

//        [Test]
//        public void Compare_Heading_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<h1>Heading</h1>";
//            var newHTML = "<h1>Heading</h1>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Heading_And_Find_Difference_Count_As_One()
//        {
//            /*Heading text is changed in newHTML*/
//            var oldHTML = "<h1>Heading</h1>";
//            var newHTML = "<h1>Heading1</h1>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void Compare_Paragraph_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<p>Paragraph</p>";
//            var newHTML = "<p>Paragraph</p>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Paragraph_And_Find_Difference_Count_As_One()
//        {
//            /*Paragraph text is changed in newHTML*/
//            var oldHTML = "<p>Paragraph</p>";
//            var newHTML = "<p>Paragraph1</p>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }   

//        [Test]
//        public void Compare_List_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<ul><li>Item1</li><li>Item2</li></ul>";
//            var newHTML = "<ul><li>Item1</li><li>Item2</li></ul>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_List_And_Find_Difference_Count_As_One()
//        {
//            /*List item is changed in newHTML*/
//            var oldHTML = "<ul><li>Item1</li><li>Item2</li></ul>";
//            var newHTML = "<ul><li>Item1</li><li>Item3</li></ul>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void Compare_Link_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<a href=\"https://www.google.com\">Google</a>";
//            var newHTML = "<a href=\"https://www.google.com\">Google</a>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Link_And_Find_Difference_Count_As_One()
//        {
//            /*Link text is changed in newHTML*/
//            var oldHTML = "<a href=\"https://www.google.com\">Google</a>";
//            var newHTML = "<a href=\"https://www.google.com\">Google1</a>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void Compare_Button_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<button>Click</button>";
//            var newHTML = "<button>Click</button>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Button_And_Find_Difference_Count_As_One()
//        {
//            /*Button text is changed in newHTML*/
//            var oldHTML = "<button>Click</button>";
//            var newHTML = "<button>Click1</button>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void Compare_Input_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<input type=\"text\" value=\"Text\">";
//            var newHTML = "<input type=\"text\" value=\"Text\">";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Input_And_Find_Difference_Count_As_One()
//        {
//            /*Input value is changed in newHTML*/
//            var oldHTML = "<input type=\"text\" value=\"Text\">";
//            var newHTML = "<input type=\"text\" value=\"Text1\">";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void Compare_Span_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<span>Span</span>";
//            var newHTML = "<span>Span</span>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Span_And_Find_Difference_Count_As_One()
//        {
//            /*Span text is changed in newHTML*/
//            var oldHTML = "<span>Span</span>";
//            var newHTML = "<span>Span1</span>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void Compare_Div_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<div>Div</div>";
//            var newHTML = "<div>Div</div>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Div_And_Find_Difference_Count_As_One()
//        {
//            /*Div text is changed in newHTML*/
//            var oldHTML = "<div>Div</div>";
//            var newHTML = "<div>Div1</div>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void Compare_Section_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<section>Section</section>";
//            var newHTML = "<section>Section</section>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Section_And_Find_Difference_Count_As_One()
//        {
//            /*Section text is changed in newHTML*/
//            var oldHTML = "<section>Section</section>";
//            var newHTML = "<section>Section1</section>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }
        
//        [Test]
//        public void Compare_Span_And_Find_Difference_Count_As_Two()
//        {
//            /*Span text is changed and style attribute is added in newHTML*/
//            var oldHTML = "<span>Span</span>";
//            var newHTML = "<span style=\"color:red\">Span1</span>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(2));
//        }

//        [Test]
//        public void Compare_Div_And_Find_Difference_Count_As_Two()
//        {
//            /*Div text is changed and style attribute is added in newHTML*/
//            var oldHTML = "<div>Div</div>";
//            var newHTML = "<div style=\"color:red\">Div1</div>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(2));
//        }

//        [Test]
//        public void Compare_Section_And_Find_Difference_Count_As_Two()
//        {
//            /*Section text is changed and style attribute is added in newHTML*/
//            var oldHTML = "<section>Section</section>";
//            var newHTML = "<section style=\"color:red\">Section1</section>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(2));
//        }

//        [Test]
//        public void Compare_Sub_And_Find_Difference_Count_As_Zero()
//        {
//            /*No Difference in the test data*/
//            var oldHTML = "<sub>Sub</sub>";
//            var newHTML = "<sub>Sub</sub>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(0));
//        }

//        [Test]
//        public void Compare_Sub_And_Find_Difference_Count_As_One()
//        {
//            /*Sub text is changed in newHTML*/
//            var oldHTML = "<sub>Sub</sub>";
//            var newHTML = "<sub>Sub1</sub>";

//            var htmlDiff = new HTMLDiff();
//            var result = htmlDiff.GetHtmlDiffs(oldHTML, newHTML);

//            Assert.That(result.Count, Is.EqualTo(1));
//        }
//    }
//}