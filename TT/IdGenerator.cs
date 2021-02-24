namespace TT
{
    public class IdGenerator
    {
        private static IdGenerator _instance;
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz~!@#$%^&*()_+|[];',./{}:<>?";
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
            if (_cursor == _chars.Length) _cursor = 0;
            return _chars[_cursor];
        }
    }
}
