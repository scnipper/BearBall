namespace Util.Touch.Impl
{
    public interface ITouchListener
    {
        public void OnTouchDown(int indexTouch);
        public void OnTouchUp(int indexTouch);
        public void OnTouchDrag(int indexTouch);
    }
}