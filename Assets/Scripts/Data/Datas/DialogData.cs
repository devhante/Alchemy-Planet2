namespace AlchemyPlanet.Data
{
    public class DialogData
    {
        public string name;
        public string content;
        public Illust[] illusts = new Illust[2];

        public DialogData(string name, string content, Illust[] illusts)
        {
            this.name = name;
            this.content = content;
            this.illusts = illusts;
        }
    }

    public enum IllistPos { Left = 0, Center, Right }
    public enum IllustMode { Front = 0, Back }
    public struct Illust
    {
        public string name;
        public IllistPos pos;
        public IllustMode mode;

        public Illust(string name, IllistPos pos, IllustMode mode)
        {
            this.name = name;
            this.pos = pos;
            this.mode = mode;
        }
    }
}