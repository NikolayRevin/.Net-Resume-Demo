using Demo.Core.Exceptions;
using Demo.Core.Models.User;

namespace Demo.Api.Models.User
{
    public class UserVoteRequest
    {
        public int Vote { get; set; }

        public UserVoteAction ToAction(string userId, string votedUserId)
        {
            if(Vote < 1 || Vote > 5)
            {
                throw new BadRequestApiException("Оценка должна быть в диапозоне от 1 до 5");
            }

            var action = new UserVoteAction
            {
                UserId = userId,
                VotedUserId = votedUserId,
                Vote = Vote
            };

            return action;
        }
    }
}
