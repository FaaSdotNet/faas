using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Project : IEquatable<Project>
    {

        /// <summary>
        /// <see cref="Id"/> is marked as (primary) key and while being a <see cref="Guid"/>, also DB generated. For <c>int</c> keys, autoincrement should be used by default.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Index("IX_ProjectUnique", 1, IsUnique = true)]
        public string Name { get; set; }

        [ForeignKey("User")]
        [Index("IX_ProjectUnique", 2, IsUnique = true)]
        public Guid UserId { get; set; }
        
        [Required]
        public User User { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Collection of all forms for a project. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Form> Forms { get; set; } = new List<Form>();

        public bool Equals(Project other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && UserId.Equals(other.UserId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Project) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0)*397) ^ UserId.GetHashCode();
            }
        }

        public static bool operator ==(Project left, Project right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Project left, Project right)
        {
            return !Equals(left, right);
        }
    }
}
