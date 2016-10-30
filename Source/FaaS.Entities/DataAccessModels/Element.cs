using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Element : IEquatable<Element>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, Index("IX_ElementUnique", 1, IsUnique = true), MaxLength(254)]
        public string Name { get; set; }

        [ForeignKey("Form"), Index("IX_ElementUnique", 2, IsUnique = true)]
        public Guid FormId { get; set; }

        [Required]
        public Form Form { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public bool Mandatory { get; set; }

        /// <summary>
        /// Collection of all values for an element. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<ElementValue> ElementValues { get; set; } = new List<ElementValue>();

        public bool Equals(Element other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && FormId.Equals(other.FormId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Element) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0)*397) ^ FormId.GetHashCode();
            }
        }

        public static bool operator ==(Element left, Element right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Element left, Element right)
        {
            return !Equals(left, right);
        }
    }
}
