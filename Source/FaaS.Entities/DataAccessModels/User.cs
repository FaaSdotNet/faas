using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class User : IEquatable<User>
    {
        /// <summary>
        /// <see cref="Id"/> is marked as (primary) key and while being a <see cref="Guid"/>, also DB generated. For <c>int</c> keys, autoincrement should be used by default.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, MaxLength(254)]
        public string Name { get; set; }
        [Required, Index(IsUnique = true)]
        public string GoogleId { get; set; }

        [Required]
        public DateTime Registered { get; set; }

        /// <summary>
        /// Collection of all projects for user. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || string.Equals(GoogleId, other.GoogleId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return GoogleId?.GetHashCode() ?? 0;
        }

        public static bool operator ==(User left, User right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !Equals(left, right);
        }
    }
}
