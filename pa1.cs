using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {
        static string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
        static bool[ , ] board; // false if tile removed; this saves row and col #s
        static bool playerATurn = true;
        static string firstName;
        static string secondName;
        static int startARow; //starting platforms
        static int startACol;
        static int startBRow;
        static int startBCol;
        static int locateARow; //save  A and B locations
        static int locateACol;
        static int locateBRow;
        static int locateBCol;
        
        static void Main( )
        {
            initialize();
            
            bool gameRun = true; //no need to check if game over
            
            while (gameRun)
            {
                DrawGameBoard( );
                if (playerATurn)
                {
                    WriteLine(firstName+": Enter a move (format: abcd - ab is row and col of new location, cd is row and col of block to remove)");
                }
                else
                {
                    WriteLine(secondName+": Enter a move (format: abcd - ab is row and col of new location, cd is row and col of block to remove)");
                }
                string move = ReadLine();
                
                while (!makeMove(move))
                {
                    move = ReadLine();
                }
                
                playerATurn = !playerATurn;
            }
        }
        
        static void initialize()
        {
            WriteLine("Welcome to Isolation!");
            WriteLine("How to play: Each player can move to a tile directly above, below, adjacent, or diagonal to their position.");
            WriteLine("Each player also chooses a tile to remove. They cannot remove a tile that a player is currently on, has a starting platform, or has already been removed.");
            WriteLine("The goal of the game is to isolate your opponent so that they cannot make another move.");
            WriteLine("Users will enter their moves in 'abcd' format, ab being row and column of pawn move, and cd is the tile to be removed.");
            WriteLine();
            
            Write( "Enter your name [default Player A]: " );
            firstName = ReadLine( );
            if( firstName.Length == 0 ) firstName = "Player A";
            WriteLine( "name: {0}", firstName );
            
            Write( "Enter your name [default Player B]: " );
            secondName = ReadLine( );
            if( secondName.Length == 0 ) secondName = "Player B";
            WriteLine( "name: {0}", secondName );
            
            WriteLine("Enter the number of rows you want. (Type 0 for default.)");
            int userRows = int.Parse(ReadLine());
            if( userRows == 0 ) userRows = 6;
            WriteLine("Enter the number of columns you want. (Type 0 for default.)");
            int userCols = int.Parse(ReadLine());
            if( userCols == 0 ) userCols = 8;
            
            board = new bool[userRows, userCols];
            
            WriteLine(firstName+ ": Enter the row and col of your starting position (format: ab)");
            string start1 = ReadLine();
            if( start1.Length == 0 && board.GetLength(0) >= 3 && board.GetLength(1) >= 1) start1 = "ca";
            while (start1.Length == 0) 
            {   
                WriteLine("Enter a valid row and col of your starting position.");
                start1 = ReadLine();
            }
            int a = Array.IndexOf(letters, start1.Substring(0,1));
            int b = Array.IndexOf(letters, start1.Substring(1,1));
            startARow = a;
            startACol = b;
            board[a, b] = true;
            
            WriteLine(secondName+ ": Enter the row and col of your starting position (format: ab)");
            string start2 = ReadLine();
            if( start2.Length == 0 && board.GetLength(0) >= 4 && board.GetLength(1) >= 8) start2 = "dh";
            while (start2.Length == 0) 
            {   
                WriteLine("Enter a valid row and col of your starting position.");
                start2 = ReadLine();
            }
            int c = Array.IndexOf(letters, start2.Substring(0,1));
            int d = Array.IndexOf(letters, start2.Substring(1,1));
            startBRow = c;
            startBCol = d;
            board[c, d] = true;
            
            locateARow = startARow;
            locateACol = startACol;
            locateBRow = startBRow;
            locateBCol = startBCol;
            
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i,j] = true;
                }
            }
            
        }
        
        static bool makeMove(string move)
        {
            //check for invalid input: length
            if (move.Length != 4)
            {
                WriteLine("Please enter 4 letters.");
                return false;
            }
            
            int a = Array.IndexOf(letters, move.Substring(0,1)); //new pawn row
            int b = Array.IndexOf(letters, move.Substring(1,1)); //new pawn column
            int c = Array.IndexOf(letters, move.Substring(2,1));
            int d = Array.IndexOf(letters, move.Substring(3,1));
            
            //check for invalid input: coordinates
            if (a > board.GetLength(0)-1 || a < 0 || b > board.GetLength(1)-1 || b < 0 || c > board.GetLength(0)-1 || c < 0 || d > board.GetLength(1)-1 || d < 0)
            {
                WriteLine("Please enter coordinates that are within the game board.");
                return false;
            }
            
            //pawn cannot move to other pawn's location
            if (a == locateBRow && b == locateBCol)
            {
                WriteLine("You cannot move here. Enter a move: (abcd)");
                return false;
            }
            
            //check for invalid input: pawn move
            if (playerATurn)
            {
                if (board[a,b]==false)
                {
                    WriteLine("You cannot move here. Enter a move:");
                    return false;
                }
                else if (a == locateBRow && b == locateBCol)
                {
                    WriteLine("You cannot move here. Enter a move:");
                    return false;
                }
                //top left corner
                else if (locateARow == 0 && locateACol == 0)
                {
                    // these are the places the pawn can move
                    // locateARow=0 & locateACol=1, lARow=1 & lACol=0, lARow=1 & lAcol=1
                    if (!(a == 0 && b == 1) && !(a == 1 && b == 0) &&!(a == 1 && b == 1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //top right corner
                else if (locateARow == 0 && locateACol == board.GetLength(1)-1)
                {
                    // lARow = 0,1,1 lACol = board.GetLength(1)-2, board.GetLength(1)-2, board.GetLength(1)-1
                    if (!(a == 0 && b == board.GetLength(1)-2) && !(a == 1 && b == board.GetLength(1)-2) && !(a == 1 && b == board.GetLength(1)-1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //topmost row (not corners)
                else if (locateARow == 0)
                {
                    // lARow = 0,1,1,1,0 lACol = locateACol-1,locateACol-1,locateACol,locateACol+1,locateACol+1
                    if (!(a == 0 && b == locateACol-1) && !(a == 1 && b == locateACol-1) && !(a == 1 && b == locateACol) && !(a == 1 && b == locateACol+1) && !(a == 0 && b == locateACol+1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //bottom left corner
                else if (locateARow == board.GetLength(0)-1 && locateACol == 0)
                {
                    // lARow = board.GetLength(0)-2,board.GetLength(0)-2,board.GetLength(0)-1 lACol=0,1,1
                    if (!(a == board.GetLength(0)-2 && b == 0) && !(a == board.GetLength(0)-2 && b == 1) && !(a == board.GetLength(0)-1 && b == 1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //bottom right corner
                else if (locateARow == board.GetLength(0)-1 && locateACol == board.GetLength(1)-1)
                {
                    // lARow = board.GetLength(0)-2,board.GetLength(0)-2,board.GetLength(0)-1 lACol = board.GetLength(1)-1,board.GetLength(1)-2,board.GetLength(1)-2
                    if (!(a == board.GetLength(0)-2 && b == board.GetLength(1)-1) && !(a == board.GetLength(0)-2 && b == board.GetLength(1)-2) && !(a == board.GetLength(0)-1 && b == board.GetLength(1)-2))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //bottommost row (not corners)
                else if (locateARow == board.GetLength(0)-1)
                {
                    // lARow = board.GetLength(0)-1,board.GetLength(0)-2,board.GetLength(0)-2,board.GetLength(0)-2,board.GetLength(0)-1 lACol = locateACol-1,locateACol-1,locateACol,locateACol+1,locateACol+1
                    if (!(a == board.GetLength(0)-1 && b == locateACol-1) && !(a == board.GetLength(0)-2 && b == locateACol-1) && !(a == board.GetLength(0)-2 && b == locateACol) && !(a == board.GetLength(0)-2 && b == locateACol+1) && !(a == board.GetLength(0)-1 && b == locateACol+1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //left column (not corners)
                else if (locateACol == 0)
                {
                    // lARow = locateARow-1,locateARow-1,locateARow,locateARow+1,locateARow+1 lACol = 0,1,1,1,0
                    if (!(a == locateARow-1 && b == 0) && !(a == locateARow-1 && b == 1) && !(a == locateARow && b == 1) && !(a == locateARow+1 && b == 1) && !(a == locateARow+1 && b == 0))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //right column (not corners)
                else if (locateACol == board.GetLength(1))
                {
                    //lARow = locateARow-1,locateARow-1,locateARow,locateARow+1,locateARow+1 LACol = board.GetLength(1)-1,board.GetLength(1)-2,board.GetLength(1)-2,board.GetLength(1)-2,board.GetLength(1)-1
                    if (!(a == locateARow-1 && b == board.GetLength(1)-1) && !(a == locateARow-1 && b == board.GetLength(1)-2) && !(a == locateARow && b == board.GetLength(1)-2) && !(a == locateARow+1 && b == board.GetLength(1)-2) && !(a == locateARow+1 && b == board.GetLength(1)-1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //middle piece 
                else
                {
                    //lARow = lARow-1,lARow-1,lARow-1,lARow,lARow,lARow+1,lARow+1,lARow+1 lACol = lACol-1,lACol,lACol+1,lACol-1,lACol+1,lACol-1,lACol,lACol+1
                    if (!(a == locateARow-1 && b == locateACol-1) && !(a == locateARow-1 && b == locateACol) && !(a == locateARow-1 && b == locateACol+1) && !(a == locateARow && b == locateACol-1) && !(a == locateARow && b == locateACol+1) && !(a == locateARow+1 && b == locateACol-1) && !(a == locateARow+1 && b == locateACol) && !(a == locateARow+1 && b == locateACol+1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                locateARow = a;
                locateACol = b;
                
                //check for invalid input: removed tile (cannot remove platforms)
                if ((c == locateBRow && d == locateBCol) || (c == startARow && d == startACol) || (c == startBRow && d == startBCol) || (board[c,d] == false) || (c == a && d == b))
                {
                    WriteLine("You cannot remove this tile. Enter a move:");
                    return false;
                }
            }
            else
            {
                if (board[a,b]==false)
                {
                    WriteLine("You cannot move here. Enter a move:");
                    return false;
                }
                else if (a == locateARow && b == locateACol)
                {
                    WriteLine("You cannot move here. Enter a move:");
                    return false;
                }
                //top left corner
                else if (locateBRow == 0 && locateBCol == 0)
                {
                    if (!(a == 0 && b == 1) && !(a == 1 && b == 0) &&!(a == 1 && b == 1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //top right corner
                else if (locateBRow == 0 && locateBCol == board.GetLength(1)-1)
                {
                    if (!(a == 0 && b == board.GetLength(1)-2) && !(a == 1 && b == board.GetLength(1)-2) && !(a == 1 && b == board.GetLength(1)-1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //top row (not corners)
                else if (locateBRow == 0)
                {
                    if (!(a == 0 && b == locateBCol-1) && !(a == 1 && b == locateBCol-1) && !(a == 1 && b == locateBCol) && !(a == 1 && b == locateBCol+1) && !(a == 0 && b == locateBCol+1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //bottom left corner
                else if (locateBRow == board.GetLength(0)-1 && locateBCol == 0)
                {
                    if (!(a == board.GetLength(0)-2 && b == 0) && !(a == board.GetLength(0)-2 && b == 1) && !(a == board.GetLength(0)-1 && b == 1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //bottom right corner
                else if (locateBRow == board.GetLength(0)-1 && locateBCol == board.GetLength(1)-1)
                {
                    if (!(a == board.GetLength(0)-2 && b == board.GetLength(1)-1) && !(a == board.GetLength(0)-2 && b == board.GetLength(1)-2) && !(a == board.GetLength(0)-1 && b == board.GetLength(1)-2))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //bottom row (not corners)
                else if (locateBRow == board.GetLength(0)-1)
                {
                    if (!(a == board.GetLength(0)-1 && b == locateBCol-1) && !(a == board.GetLength(0)-2 && b == locateBCol-1) && !(a == board.GetLength(0)-2 && b == locateBCol) && !(a == board.GetLength(0)-2 && b == locateBCol+1) && !(a == board.GetLength(0)-1 && b == locateBCol+1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //left column (not corners)
                else if (locateBCol == 0)
                {
                    if (!(a == locateBRow-1 && b == 0) && !(a == locateBRow-1 && b == 1) && !(a == locateBRow && b == 1) && !(a == locateBRow+1 && b == 1) && !(a == locateBRow+1 && b == 0))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //right column (not corners)
                else if (locateBCol == board.GetLength(1))
                {
                    if (!(a == locateBRow-1 && b == board.GetLength(1)-1) && !(a == locateBRow-1 && b == board.GetLength(1)-2) && !(a == locateBRow && b == board.GetLength(1)-2) && !(a == locateBRow+1 && b == board.GetLength(1)-2) && !(a == locateBRow+1 && b == board.GetLength(1)-1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                //middle piece
                else
                {
                    if (!(a == locateBRow-1 && b == locateBCol-1) && !(a == locateBRow-1 && b == locateBCol) && !(a == locateBRow-1 && b == locateBCol+1) && !(a == locateBRow && b == locateBCol-1) && !(a == locateBRow && b == locateBCol+1) && !(a == locateBRow+1 && b == locateBCol-1) && !(a == locateBRow+1 && b == locateBCol) && !(a == locateBRow+1 && b == locateBCol+1))
                    {
                        WriteLine("Please enter a valid move in relation to your pawn.");
                        return false;
                    }
                }
                
                //check for invalid input: removed tile (cannot remove platforms)
                if ((c == locateARow && d == locateACol) || (c == startARow && d == startACol) || (c == startBRow && d == startBCol) || (board[c,d] == false) || (c == a && d == b))
                {
                    WriteLine("You cannot remove this tile. Enter a move:");
                    return false;
                }
                locateBRow = a;
                locateBCol = b;
            }
            
            board[c,d] = false;
            return true;
        }
        
        static void DrawGameBoard( )
        {
            Clear();
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string bb = "\u25a0"; // block
            const string fb = "\u2588"; // full block
            const string lh = "\u258c"; // left half block
            const string rh = "\u2590"; // right half block
                
            // Label columns    
            Write("   ");    
            for( int c = 0; c < board.GetLength( 1 ); c ++ )
            {
                Write( "  {0} ", letters[ c ] );
            }
            WriteLine();
            
            // Draw the top board boundary.
            Write( "   " );
            for( int c = 0; c < board.GetLength( 1 ); c ++ )
            {
                if( c == 0 ) Write( tl );
                Write( "{0}{0}{0}", h );
                if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", tr ); 
                else                                Write( "{0}", hb );
            }
            WriteLine( );
            
            // Draw the board rows.
            for( int r = 0; r < board.GetLength( 0 ); r ++ )
            {
                Write( " {0} ", letters[ r ] );
                
                // Draw the row contents.
                for( int c = 0; c < board.GetLength( 1 ); c ++ )
                {
                    if (c==0) Write( v );
                    if( board[ r, c ] ) 
                    {   
                        if (r == locateARow && c == locateACol)
                        {
                            Write( "{0}{1}", " A", " "+v );
                        }
                        else if (r == locateBRow && c == locateBCol)
                        {
                            Write( "{0}{1}", " B", " "+v );
                        }
                        else if (r == startARow && c == startACol)
                        {
                            Write( "{0}{1}", " "+bb, " "+v );
                        }
                        else if (r == startBRow && c == startBCol)
                        {
                            Write( "{0}{1}", " "+bb, " "+v );
                        }
                        else
                        {
                            Write( "{0}{1}", rh+fb+lh, v );
                        }
                    }
                    else
                    {
                        if ((r == startARow && c == startACol) || (r == startBRow && c == startBCol))
                            Write( "{0}{1}", " "+bb, " "+v );
                        else
                            Write( "{0}{1}", "   ", v );
                    }
                }
                WriteLine( );
                
                // Draw the boundary after the row.
                if( r != board.GetLength( 0 ) - 1 )
                { 
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( vr );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", vl ); 
                        else                                Write( "{0}", hv );
                    }
                    WriteLine( );
                }
                else
                {
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( bl );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", br ); 
                        else                                Write( "{0}", ha );
                    }
                    WriteLine( );
                }
            }
        }
    }
}
