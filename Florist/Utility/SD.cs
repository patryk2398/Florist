﻿using Florist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Florist.Utility
{
    public static class SD
    {
        public const string DefaultFlowerImage = "default.png";
        public const string ManagerUser = "Manager";
        public const string CustomerEndUser = "Customer";
        public const string ssShoppingCartCount = "ssCartCount";
        public const string ssCouponCode = "ssCouponCode";

        public const string StatusSubmitted = "PRZYJĘTO";
        public const string StatusInProcess = "W PRZYGOTOWANIU";
        public const string StatusReady = "DO ODBIORU";
        public const string StatusCompleted = "ODEBRANO";
        public const string StatusCancelled = "ANULOWANO";

        public const string PaymentStatusPending = "OCZEKUJE";
        public const string PaymentStatusApproved = "ZATWIERDZONO";
        public const string PaymentStatusRejected = "ODRZUCONO";

        public const string Completed = "Completed.png";
        public const string InKitchen = "InKitchen.png";
        public const string OrderPlaced = "OrderPlaced.png";
        public const string ReadyForPickup = "ReadyForPickup.png";

        public static double DiscountedPrice(Coupon couponFromDb, double OriginallOrderTotal)
        {
            if (couponFromDb == null)
            {
                return OriginallOrderTotal;
            }
            else
            {
                if (couponFromDb.MinimumAmount > OriginallOrderTotal)
                {
                    return OriginallOrderTotal;
                }
                else
                {
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.PLN)
                    {
                        return Math.Round(OriginallOrderTotal - couponFromDb.Discount, 2);
                    }
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.Procent)
                    {
                        return Math.Round(OriginallOrderTotal - (OriginallOrderTotal * couponFromDb.Discount / 100), 2);
                    }
                }
            }
            return OriginallOrderTotal;
        }
    }
}
