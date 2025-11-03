using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ECommerce.AuthService.Application.Events;

namespace ECommerce.AuthService.Application.Services
{
    public class EmailSentNotificationService
    {
        // Luu cac TaskCompletionSource cho cac userId dang doi email sent event
        private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> _pendingRegistrations = new();

        public void NotifyEmailSent(EmailSentEvent emailEvent)
        {
            // Neu co registration dang doi cho userId nay, signal thanh cong
            if (_pendingRegistrations.TryRemove(emailEvent.UserId, out var tcs))
            {
                tcs.SetResult(emailEvent.Success);
            }
        }

        public Task<bool> WaitForEmailSentAsync(string userId, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource<bool>();
            _pendingRegistrations.AddOrUpdate(userId, tcs, (key, oldValue) => tcs);

            // Timeout sau khoang thoi gian quy dinh
            Task.Delay(timeout).ContinueWith(_ =>
            {
                if (_pendingRegistrations.TryRemove(userId, out var removedTcs))
                {
                    // Chi set false neu chua duoc set result tu email event
                    removedTcs.TrySetResult(false);
                }
            });

            return tcs.Task;
        }
    }
}

