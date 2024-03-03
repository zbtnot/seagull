using System.Linq.Expressions;
using Moq;
using Seagull.Model;
using Seagull.Service;
using Seagull.Service.Contract;

namespace test;

[TestClass]
public class IndexGeneratorTest
{
    private readonly Mock<IHtmlTemplateParser> _htmlTemplateParser = new();

    [TestMethod]
    public void TestGenerateIndex()
    {
        var pages = new List<Page>
        {
            new Page
            {
                Content = "content goes here",
                Title = "A title",
                Description = "A description of a post",
            }
        };
        
        var rendereredIndex = 
            $"""
                {pages[0].Title}
                {pages[0].Description}
             """;

        Expression<Func<IHtmlTemplateParser, HtmlTemplate>> parse = parser => parser.Parse(It.IsAny<string>());
        _htmlTemplateParser.Setup(parse).Returns(new HtmlTemplate());

        Expression<Func<IHtmlTemplateParser, string>> render = parser =>
            parser.Render(It.IsAny<HtmlTemplate>(), It.IsAny<IDictionary<object, object>>());
        _htmlTemplateParser.Setup(render).Returns(rendereredIndex);

        var indexGenerator = new IndexGenerator(_htmlTemplateParser.Object);
        var index = indexGenerator.Generate(pages, string.Empty, new Configuration());
        
        Assert.IsTrue(index.Contains(pages[0].Title));
        Assert.IsTrue(index.Contains(pages[0].Description));
        _htmlTemplateParser.Verify(parse, Times.Once);
        _htmlTemplateParser.Verify(render, Times.Once);
    }
}
