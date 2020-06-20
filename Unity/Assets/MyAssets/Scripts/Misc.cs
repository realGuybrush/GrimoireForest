public class Pnt
{
    private readonly float x = 0.0f;
    private readonly float y = 0.0f;

    public bool Near(float X, float Y)
    {
        if (X > x - 0.01f && X < x + 0.01f && Y > y - 0.01f && Y < y + 0.01f)
        {
            return true;
        }

        return false;
    }

    /*spublic bool Near(int X, int Y)
    {
        if((X>(x-0.01f))&&(X<(x+0.01f))&&(Y>(y-0.01f))&&(Y<(y+0.01f)))
        {
            return true;
        }
        return false;
    }

    public bool Near(Pnt p)
    {
        if((p.x>(x-0.01f))&&(p.x<(x+0.01f))&&(p.y>(y-0.01f))&&(p.y<(y+0.01f)))
        {
            return true;
        }
        return false;
    }*/
}