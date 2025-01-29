namespace ProjectName.Service;

public class LoginAttemptService
{
    private readonly Dictionary<string, LoginAttempt> _failedLoginAttempts = new();

    private const int MaxFailedAttempts = 3;
    private readonly TimeSpan _lockoutTime = TimeSpan.FromMinutes(15);

    // Check User if he is locked out
    public bool IsUserLockedOut(string username)
    {
        if (_failedLoginAttempts.TryGetValue(username, out var attempt))
        {
            if (attempt.LockoutEnd.HasValue && attempt.LockoutEnd.Value > DateTime.UtcNow)
            {
                return true; // User is locked out
            }
        }
        return false; // Not locked out
    }

    // Record Times tried 
    public void RecordFailedAttempt(string username)
    {
        if (!_failedLoginAttempts.ContainsKey(username))
        {
            _failedLoginAttempts[username] = new LoginAttempt { FailedAttempts = 1 };
        }
        else
        {
            _failedLoginAttempts[username].FailedAttempts++;
            if (_failedLoginAttempts[username].FailedAttempts >= MaxFailedAttempts)
            {
                _failedLoginAttempts[username].LockoutEnd = DateTime.UtcNow.Add(_lockoutTime);
            }
        }
    }
    // Reset the failed attempts
    public void ResetFailedAttempts(string username)
    {
        if (_failedLoginAttempts.ContainsKey(username))
        {
            _failedLoginAttempts.Remove(username); // Reset on successful login
        }
    }
    // Check time remaining
    public TimeSpan? GetLockoutTimeRemaining(string username)
    {
        if (_failedLoginAttempts.TryGetValue(username, out var attempt) && attempt.LockoutEnd.HasValue)
        {
            return attempt.LockoutEnd.Value - DateTime.UtcNow;
        }
        return null;
    }
}

public class LoginAttempt
{
    public int FailedAttempts { get; set; }
    public DateTime? LockoutEnd { get; set; }
}