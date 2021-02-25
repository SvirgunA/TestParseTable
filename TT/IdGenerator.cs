namespace TT
{
    public class IdGenerator
    {
        private static IdGenerator _instance;
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz~!@#$%^&*()_+|[];',./{}:<>?";
        private int _cursor;

        private IdGenerator()
        {
            _cursor = 0;
        }
 
        public static IdGenerator GetInstance()
        {
            if (_instance == null)
                _instance = new IdGenerator();
            return _instance;
        }

        public char GetId()
        {
            _cursor++;
            if (_cursor == Chars.Length) _cursor = 0;
            return Chars[_cursor];
        }
    }
}
