using System;
using System.Runtime;
using System.Collections;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace CNC.Common.Utils
{
    public static class Networking
    {
        public static void SendData_PlayerLifeMana(Player player, int to = -1, int from = -1) => SendData_PlayerLifeMana(player.whoAmI, to, from);
        public static void SendData_PlayerLifeMana(int playerID, int to = -1, int from = -1)
        {
            NetworkText text = null;
            NetMessage.SendData(MessageID.PlayerLifeMana, to, from, text, playerID, 0f, 0f, 0f, 0, 0, 0);
        }
    }
}