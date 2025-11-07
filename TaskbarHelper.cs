public static class TaskbarHelper
{
    public static TaskbarPosition GetTaskbarPosition()
    {
        // Windows API ile taskbar pozisyonunu tespit et
        // Örnek implementasyon...
        return TaskbarPosition.Bottom; // veya tespit edilen pozisyon
    }
    
    public static bool IsVerticalTaskbar()
    {
        var position = GetTaskbarPosition();
        return position == TaskbarPosition.Left || position == TaskbarPosition.Right;
    }
}