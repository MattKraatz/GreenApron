using System;
using System.Collections.Generic;
using WebAPI;
using Xunit;

namespace Tests.WebAPI
{
    public class PlanTests
    {
        private TaskManager _task { get; set; } = new TaskManager();

        public PlanTests()
        {
        }
        
        [Fact]
        public async void CanAddaPlan()
        {
            var response = await _task.AddPlan();
            Assert.NotNull(response);
            Assert.True(response.success);
            var plans = await _task.GetPlans();
            await _task.DeletePlan(plans.plans[0].PlanId);
        }

        [Fact]
        public async void CanDeleteAPlan()
        {
            await _task.AddPlan();
            var plans = await _task.GetPlans();
            await _task.DeletePlan(plans.plans[0].PlanId);
            var plansAgain = await _task.GetPlans();
            Assert.NotNull(plansAgain);
            Assert.False(plansAgain.success);
        }

        [Fact]
        public async void CanGetAllPlans()
        {
            await _task.AddPlan();
            var plans = await _task.GetPlans();
            Assert.NotNull(plans);
            Assert.True(plans.plans.Count > 0);
            foreach (Plan plan in plans.plans)
            {
                await _task.DeletePlan(plan.PlanId);
            }
        }

        [Fact]
        public async void CanCompleteAPlan()
        {
            await _task.AddPlan();
            var plans = await _task.GetPlans();
            var plan = plans.plans[0];
            plan.DateCompleted = DateTime.Now;
            var response = await _task.UpdatePlan(plan);
            Assert.NotNull(response);
            Assert.True(response.success);
            foreach (Plan dbPlan in plans.plans)
            {
                await _task.DeletePlan(dbPlan.PlanId);
            }
        }
    }
}
