using System;
using UnityEngine;

public enum CollisionType { Floor, Wall, Ceiling }

public class CharacterCollision2D
{
    public CollisionType type;
    public Vector2 normal;
    public Collider2D collider;

    public CharacterCollision2D(CollisionType type, Vector2 normal, Collider2D collider)
    {
        this.type = type;
        this.normal = normal;
        this.collider = collider;
    }

    public CharacterCollision2D(ContactPoint2D contact, Func<ContactPoint2D, CollisionType> categorizer)
    {
        this.type = categorizer(contact);
        this.normal = contact.normal;
        this.collider = contact.collider;
    }
}