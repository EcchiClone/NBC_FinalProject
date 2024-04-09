using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Task/Category", fileName = "Category_")]
public class TaskCategory : ScriptableObject, IEquatable<TaskCategory>
{
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;

    public string CodeName => codeName;
    public string DisplayName => displayName;

    #region Operator
    public bool Equals(TaskCategory other)
    {
        if (other is null) return false;
        if (ReferenceEquals(other, this)) return true;
        if (GetType() != other.GetType()) return false;
        return codeName == other.CodeName;
    }
    public override int GetHashCode() => (CodeName, DisplayName).GetHashCode();
    public override bool Equals(object other) => base.Equals(other);
    public static bool operator ==(TaskCategory lhs, string rhs)
    {
        if (lhs is null) return ReferenceEquals(rhs, null);
        return lhs.CodeName == rhs || lhs.DisplayName == rhs;
    }
    public static bool operator !=(TaskCategory lhs, string rhs) => !(lhs == rhs);
    // category.CodeName == "Kill" 이렇게 할 필요 없이
    // category == "Kill" 처럼 작성 할 수 있게 하였음.
    #endregion
}
