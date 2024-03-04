namespace Seagull.Model;

/**
 * Represents a MarkdownFile's parsed and extracted data.
 */
public class Page
{
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime Date { get; init; } = DateTime.Today;
}
