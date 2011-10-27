using System.Text;

namespace fBot {
    class Color {
        public static string StripColors( string text ) {
            StringBuilder output = new StringBuilder();
            for( int i = 0; i < text.Length; i++ ) {
                if( text[i] == '&' ) {
                    if( i < text.Length - 1 && text[i + 1] == '&' ) {
                        output.Append( '&' );
                    } else {
                        i++;
                    }
                } else {
                    output.Append( text[i] );
                }
            }
            return output.ToString();
        }
    }
}