﻿using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class ExchangeDAO
    {
        private static readonly string tableName = "ExchangeConfig";

        public static void AddRecord(ExchangeConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<ExchangeConfig>(tableName);
                var insertResult = table.Insert(new ExchangeConfig()
                {
                    Exchange = record.Exchange,
                    ApiKey = record.ApiKey,
                    ApiSecret = record.ApiSecret,
                    DestinationCoin = record.DestinationCoin,
                    ExcludedSourceCoins = record.ExcludedSourceCoins,
                    TradingPairCurrency = record.TradingPairCurrency,
                    Enabled = record.Enabled
                });
            }
        }

        public static void DeleteRecord(ExchangeConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<ExchangeConfig>(tableName);
                table.Delete(record.Id);
            }
        }

        public static List<ExchangeConfig>? GetRecords()
        {
            List<ExchangeConfig>? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<ExchangeConfig>(tableName);
                result = table.FindAll().ToList();
            }
            return result;
        }

        public static ExchangeConfig? GetRecord(int id)
        {
            ExchangeConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<ExchangeConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static void UpdateRecord(ExchangeConfig record)
        {
            var existingRecord = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<ExchangeConfig>(tableName);
                if (existingRecord != null)
                {
                    existingRecord.Enabled = record.Enabled;
                    existingRecord.Exchange = record.Exchange;
                    existingRecord.ApiKey = record.ApiKey;
                    existingRecord.ApiSecret = record.ApiSecret;
                    existingRecord.DestinationCoin = record.DestinationCoin;
                    existingRecord.Exchange = record.Exchange;
                    existingRecord.ExcludedSourceCoins = record.ExcludedSourceCoins;
                    existingRecord.TradingPairCurrency = record.TradingPairCurrency;
                    table.Update(existingRecord);
                }
                else
                {
                    table.Insert(new ExchangeConfig()
                    {
                        Exchange = record.Exchange,
                        ApiKey = record.ApiKey,
                        ApiSecret = record.ApiSecret,
                        DestinationCoin = record.DestinationCoin,
                        ExcludedSourceCoins = record.ExcludedSourceCoins,
                        TradingPairCurrency = record.TradingPairCurrency,
                        Enabled = record.Enabled
                    });
                }

            }
        }
    }
}
