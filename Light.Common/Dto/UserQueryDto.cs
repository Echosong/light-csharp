namespace Light.Common.Dto {
    public class UserQueryDto : PageInfo {
        public string? Pusername { get; set; }

        public string? Username { get; set; }

        public string? Name { get; set; }

        public int Type { get; set; }

        public int Level { get; set; }
    }
}