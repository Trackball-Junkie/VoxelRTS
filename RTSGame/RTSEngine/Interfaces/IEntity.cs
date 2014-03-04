﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSEngine.Data.Team;

namespace RTSEngine.Interfaces {
    public interface IEntity {
        // The Entity's Team
        RTSTeam Team { get; }

        // Location In The World
        Vector3 WorldPosition { get; }

        // Height-Agnostic Grid Location
        Vector2 GridPosition { get; set; }

        // Collision Geometry
        ICollidable CollisionGeometry { get; }

        // Targetting Information
        IEntity Target { get; set; }

        // Event Triggered When This Entity Finds A New Target (Null When Can't Find One)
        event Action<IEntity, IEntity> OnNewTarget;

        // Whether It Is Alive Or Not
        bool IsAlive { get; set; }

        // Event Triggered When Entity Is Destroyed
        event Action<IEntity> OnDestruction;

        // These Can Change During The Entity's Lifecycle But Will Always Be There
        IActionController ActionController { get; set; }
        ITargettingController TargettingController { get; set; }

        // Calls The Destruction Event
        void Destroy();
    }

    public interface IMovingEntity : IEntity {
        // Speed Of Movement For The Entity
        float MovementSpeed { get; }

        // The Direction The Entity Is Looking
        Vector2 ViewDirection { get; }

        // This Can Change During The Entity's Lifecycle But Will Always Be There
        IMovementController MovementController { get; set; }
        
        // Changes The GridPosition Of The Entity
        void Move (Vector2 change);
    }

    public interface IDestructibleEntity : IEntity {
        // The Current Health Of The Entity
        int Health { get; }

        // Applies Damage To Health
        void Damage(int d);

        // Event Triggered When This Entity Receives Damage
        event Action<IDestructibleEntity, int> OnDamage;
    }

    public interface ICombatEntity : IDestructibleEntity, IMovingEntity {

        // Computes The Damage To Deal With Access To A Random Number
        int ComputeDamage(double rand);

        // Actually Damages A Target
        void DamageTarget(double rand);

        // Event From Combat
        event Action<ICombatEntity, IDestructibleEntity> OnAttackMade;

        // This Can Change During The Entity's Lifecycle But Will Always Be There
        ICombatController CombatController { get; set; }
    }

}