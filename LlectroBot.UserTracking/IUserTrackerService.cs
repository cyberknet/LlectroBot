﻿using Discord;

namespace LlectroBot.UserTracking
{
    public interface IUserTrackerService
    {
        UserState GetUserState(IGuild guild, IUser user);
        //void UpdateUser(IGuild guild, IUser oldUser, IUser newUser);
        //void UpdateUserState(IGuild guild, IUser user, string action);
    }
}
