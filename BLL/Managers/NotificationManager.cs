﻿using GotIt.BLL.ViewModels;
using GotIt.Common.Enums;
using GotIt.Common.Helper;
using GotIt.MSSQL;
using GotIt.MSSQL.Models;
using GotIt.MSSQL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GotIt.BLL.Managers
{
    public class NotificationManager : Repository<NotificationEntity>
    {
        public NotificationManager(GotItDbContext dbContext) : base(dbContext) {}

        public Result<List<NotificationViewModel>> GetUserNotifications(int userId, int pageNo, int pageSize)
        {
            try
            {
                var notifications = GetAllPaginated(n => n.ReceiverId == userId, pageNo, pageSize, "Sender");

                if(notifications.Data == null)
                {
                    throw new Exception(EResultMessage.DatabaseError.ToString());
                }

                var result = notifications.Data.Select(n => new NotificationViewModel
                {
                    Id = n.Id,
                    Content = n.Content,
                    IsSeen = n.IsSeen,
                    Link = n.Link,
                    Type = n.Type,
                    Date = n.Date,
                    Sender = n.Sender != null ? new UserViewModel
                    {
                        Id = n.Sender.Id,
                        Name = n.Sender.Name,
                        Picture = n.Sender.Picture
                    } : null
                }).ToList();

                return ResultHelper.Succeeded(result, notifications.Count);
            }
            catch (Exception e)
            {
                return ResultHelper.Failed<List<NotificationViewModel>>(message: e.Message);
            }
        }

        public Result<bool> ReadNotification(int userId, int notificationId)
        {
            try
            {
                var model = new NotificationEntity
                {
                    Id = notificationId,
                    IsSeen = true,
                    ReceiverId = userId,
                };

                Update(model, n => n.IsSeen);
                var result = SaveChanges();

                if (!result)
                {
                    throw new Exception(EResultMessage.DatabaseError.ToString());
                }

                return ResultHelper.Succeeded(result);
            }
            catch (Exception e)
            {
                return ResultHelper.Failed(false, message: e.Message);
            }
        }

        public NotificationEntity AddNotification(int receiverId, NotificationViewModel notification)
        {
            try
            {
                var model = new NotificationEntity
                {
                    IsSeen = false,
                    Content = notification.Content,
                    Link = notification.Link,
                    ReceiverId = receiverId,
                    SenderId = notification.Sender?.Id,
                    Type = notification.Type,
                    Date = DateTime.UtcNow
                };

                return Add(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
