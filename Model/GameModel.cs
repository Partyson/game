﻿using System;
using System.Collections.Generic;
using Game.Model.EntityModel;

namespace Game.Model
{
    public class GameModel
    {
        private GameState _gameState;
        private object lockObject = new object();
        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                Console.Write("");
                GameStateChanged?.Invoke(value);
            }
        } 
        
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public List<Booster> Boosters { get; private set; }
        public int Score { get; private set; }

        public event Action<GameState> GameStateChanged;
        
        public void StartGame()
        {
            Player = new Player(0, 0, _ => GameOver());
            Score = 0;
            Enemies = new List<Enemy>();
            Boosters = new List<Booster>();
        }

        public void SpawnEnemy(int x, int y)
        {
            const int startHealth = 100;
            const int startDamage = 20;
            Enemies.Add(new Enemy(x, y, EnemyDied, startHealth, startDamage));
        }

        public void SpawnBooster(int x, int y, BoosterData boosterData)
        {
            Boosters.Add(new Booster(x, y, BoosterPicked, boosterData));
        }
        
        public void GameOver()
        {
            GameState = GameState.Menu;
        }

        private void EnemyDied(Entity enemy)
        {
            lock(lockObject)
                Enemies.Remove(enemy as Enemy);
            Score++;
        }

        private void BoosterPicked(Entity booster)
        {
            lock (lockObject)
                Boosters.Remove(booster as Booster);
        }
    }
}