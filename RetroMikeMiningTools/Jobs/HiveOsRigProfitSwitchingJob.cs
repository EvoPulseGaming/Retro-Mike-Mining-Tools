﻿using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using Quartz;
using System.Web;
using RetroMikeMiningTools.ProfitSwitching;
using RetroMikeMiningTools.Common;

namespace RetroMikeMiningTools.Jobs
{
    public class HiveOsRigProfitSwitchingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            string? platformName = context.JobDetail.JobDataMap.GetString("platform_name") ?? String.Empty;

            var config = CoreConfigDAO.GetCoreConfig();
            if (config != null && config.ProfitSwitchingEnabled)
            {
                if (String.IsNullOrEmpty(config.HiveApiKey) || String.IsNullOrEmpty(config.HiveFarmID))
                {
                    Common.Logger.Log("Skipping Hive OS Rig Profit Switching because there isno Hive API Key and/or Farm ID configured", LogType.System);
                }
                else
                {
                    Common.Logger.Log("Executing HiveOS Rig Profit Switching Job", LogType.System);
                    var rigs = HiveRigDAO.GetRecords().Where(x => x.Enabled);
                    if (rigs != null && rigs.Count() > 0)
                    {
                        foreach (var rig in rigs.Where(x => x.Enabled))
                        {
                            if (!rig.DonationRunning && !String.IsNullOrEmpty(rig.WhatToMineEndpoint) && (
                                rig.MiningMode == MiningMode.Profit ||
                                rig.MiningMode == MiningMode.CoinStacking ||
                                rig.MiningMode == MiningMode.DiversificationByProfit ||
                                rig.MiningMode == MiningMode.DiversificationByStacking
                                )
                            )
                            {
                                HiveOsGpuRigProcessor.Process(rig, config);
                            }
                            else if(!rig.DonationRunning && rig.MiningMode == MiningMode.ZergPoolAlgoProfitBasis)
                            {
                                ZergAlgoProcessor.Process(rig, config);
                            }
                        }
                    }
                }

                if (!platformName.Equals(Constants.PLATFORM_DOCKER_ARM64, StringComparison.OrdinalIgnoreCase))
                {
                    Common.Logger.Log("Executing Goldshell ASIC Profit Switching Job", LogType.System);
                    GoldshellAsicProcessor.Process(config, platformName);
                }
            }

            return Task.CompletedTask;
        }
    }
}
