using System.Linq;
using System.Collections.Generic;

App.Run();

public class Controller
{
    public void Solve(IEnumerable<Piece> pieces)
    {
        int x = 1;
        int y = 0;

        var lt = pieces.Where(p => p.IsLeftTopPiece()).First();
        pieces.ToList().Remove(lt);
        lt.SetPosition(0, 0);

        Piece actualPiece = lt;
        foreach(var piece in pieces)
        {
            if (piece.ConnectLeft(actualPiece)) {
                piece.SetPosition(x, y);
                actualPiece = piece;
                x++;
            }
        }
        
    }

}