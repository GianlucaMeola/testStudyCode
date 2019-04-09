using System;

namespace GameEngine
{
    public class NonPlayerCharacter
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FistName} {LastName}";
        public int Health { get; set; } = 100;
        public void TakeDamage(int damage)
        {
            Health = Math.Max(1, Health -= damage);
        }
    }
}
