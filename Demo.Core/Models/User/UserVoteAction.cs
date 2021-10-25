namespace Demo.Core.Models.User
{
    public class UserVoteAction
    {
        public string UserId { get; set; }

        public string VotedUserId { get; set; }

        public int Vote { get; set; }
    }
}
