using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Subscriptions bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplySubscriptionsConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // SubscriptionPlan Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<SubscriptionPlan>().HasKey(p => p.Id);
        builder.Entity<SubscriptionPlan>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SubscriptionPlan>()
            .Property(p => p.PlanType).IsRequired()
            .HasConversion<string>().HasMaxLength(30);
        builder.Entity<SubscriptionPlan>().Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Entity<SubscriptionPlan>().Property(p => p.Price).IsRequired().HasPrecision(10, 2);
        builder.Entity<SubscriptionPlan>().Property(p => p.MaxScreens).IsRequired();

        builder.Entity<SubscriptionPlan>().HasIndex(p => p.Name).IsUnique();

        // ---------------------------------------------------------------------
        // Subscription Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<Subscription>().HasKey(s => s.Id);
        builder.Entity<Subscription>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Subscription>().Property(s => s.CinemaId).IsRequired();
        builder.Entity<Subscription>().Property(s => s.PlanId).IsRequired();
        builder.Entity<Subscription>()
            .Property(s => s.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<Subscription>().Property(s => s.StartDate).IsRequired();
        builder.Entity<Subscription>().Property(s => s.EndDate).IsRequired();

        // ---------------------------------------------------------------------
        // PaymentTransaction Entity
        // ---------------------------------------------------------------------
        builder.Entity<PaymentTransaction>().HasKey(t => t.Id);
        builder.Entity<PaymentTransaction>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PaymentTransaction>().Property(t => t.Amount).IsRequired().HasPrecision(10, 2);
        builder.Entity<PaymentTransaction>()
            .Property(t => t.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<PaymentTransaction>().Property(t => t.ProcessedAt).IsRequired();

        builder.Entity<Subscription>()
            .HasMany(s => s.Transactions)
            .WithOne()
            .HasForeignKey(t => t.SubscriptionId)
            .HasConstraintName("fk_payment_transactions_subscriptions")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // SubscriptionUpgrade Entity
        // ---------------------------------------------------------------------
        builder.Entity<SubscriptionUpgrade>().HasKey(u => u.Id);
        builder.Entity<SubscriptionUpgrade>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SubscriptionUpgrade>().Property(u => u.OldPlanId).IsRequired();
        builder.Entity<SubscriptionUpgrade>().Property(u => u.NewPlanId).IsRequired();
        builder.Entity<SubscriptionUpgrade>().Property(u => u.ChangedAt).IsRequired();

        builder.Entity<Subscription>()
            .HasMany(s => s.Upgrades)
            .WithOne()
            .HasForeignKey(u => u.SubscriptionId)
            .HasConstraintName("fk_subscription_upgrades_subscriptions")
            .OnDelete(DeleteBehavior.Cascade);
    }
}