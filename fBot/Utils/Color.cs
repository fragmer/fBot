using System;
using System.Text;
using JetBrains.Annotations;

namespace fBot {
    class Color {
        public static string StripColors( [NotNull] string text ) {
            if( text == null ) throw new ArgumentNullException( "text" );
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