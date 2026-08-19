namespace WebApplication1.Model
{
    public record EmbeddedChunk(
        string Source,
        string Content,
        float[] Vector);

}
