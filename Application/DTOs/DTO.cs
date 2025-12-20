namespace Application.DTOs
{
    public class CreateListDto
    {
        public string Title { get; set; } = default!;
    }
    public class CreateBoardDto
    {
        public string Name { get; set; } = default!;
    }
    public class UpdateListDto
    {
        public string Title { get; set; } = default!;
        public int OrderIndex { get; set; }
    }

    public class CreateCardDto
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class UpdateCardDto
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class MoveCardDto
    {
        public int TargetListId { get; set; }
        public int TargetOrderIndex { get; set; }
    }
}
