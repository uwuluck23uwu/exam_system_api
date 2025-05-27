namespace EXAM_SYSTEM_API.Application.Shared
{
    public class ConvertDate
    {
        public DateTime DateOnlyToDateTime(DateOnly date)
            => date.ToDateTime(TimeOnly.MinValue);
    }
}
