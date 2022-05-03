/* zeby nie bylo konfliktow z DAL OrderState */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OS = OrderTrackingSystem.Logic.Services.OrderState;

namespace OrderTrackingSystem.Logic.HelperClasses
{
    public class FSMContext
    {
        public State State { get; set; }
        public FSMContext(OS State)
        {
            /* Map enum to FSM state */
            this.State = State switch
            {
                OS.PrepatedBySeller => new StateA(),
                OS.GetFromSeller => new StateB(),
                OS.GetByLocal => new StateC(),
                OS.SentFromLocal => new StateD(),
                OS.ToDelivery => new StateE(),
                OS.ReadyToPickup => new StateF(),
                OS.ComplaintSet => new StateG(),
                OS.ComplaintResolved => new StateH(),
                OS.ReturnToSeller => new StateI(),
                OS.Getted => new StateJ(),
                _ => null
            };
        }

        public void Request(int decision = 0)
        {
            State.Handle(this, decision);
        }
    }

    public abstract class State
    {
        public abstract void Handle(FSMContext context, int decision = 0);
        /* Tuple uzyty aby zwracac stan i jego przejscia do kolejnych stanow */
        public abstract IReadOnlyCollection<Tuple<OS,int>> GetNextStates();
    }

    /* Przygotowywana przez sprzedawce */
    public class StateA : State
    {
        public override void Handle(FSMContext context, int decision = 0)
        {
            if (decision != 0) return;
            context.State = new StateB();
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.GetFromSeller, 0) });
        }
    }

    /* Odebrana od nadawcy */
    public class StateB : State
    {
        public override void Handle(FSMContext context, int decision = 0)
        {
            if (decision != 0) return;
            context.State = new StateC();
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.GetByLocal, 0) });
        }
    }

    /* Przyjeta w oddziale */
    public class StateC : State
    {
        public override void Handle(FSMContext context, int decision)
        {
            if (decision != 0) return;
            context.State = new StateD();
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.SentFromLocal, 0) });
        }
    }

    /* Wyslana z oddzialu */
    public class StateD : State
    {
        public override void Handle(FSMContext context, int decision = 0)
        {
            if(decision == 0)
            {
                context.State = new StateE();
            }
            else if(decision == 1)
            {
                context.State = new StateC();
            }
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.ToDelivery, 0) ,
                                                                                     new Tuple<OS, int>(OS.GetByLocal, 1)});
        }

    }
    
    /* Wydana do doreczenia */
    public class StateE : State
    {
        public override void Handle(FSMContext context, int decision)
        {
            if (decision != 0) return;
            context.State = new StateF();
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.ReadyToPickup, 0) });
        }
    }

    /* Gotowa do odbioru */
    public class StateF : State
    {
        public override void Handle(FSMContext context, int decision)
        {
            if (decision != 0) return;
            context.State = new StateG();
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.ComplaintSet, 0) });
        }
    }

    /* Zalozona reklamacja */
    public class StateG : State
    {
        public override void Handle(FSMContext context, int decision)
        {
            if (decision == 0)
            {
                context.State = new StateH();
            }
            else if (decision == 1)
            {
                context.State = new StateI();
            }
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.ComplaintResolved, 0) ,
                                                                                     new Tuple<OS, int>(OS.ReturnToSeller, 1)});
        }
    }

    /* Rozwiazana reklamacja */
    public class StateH : State
    {
        public override void Handle(FSMContext context, int decision)
        {
            if (decision == 0)
            {
                context.State = new StateJ();
            }
            else if (decision == 1)
            {
                context.State = new StateI();
            }
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>> { new Tuple<OS, int>(OS.ReturnToSeller, 1) });
        }
    }

    /* Zwrocona do nadawcy */
    public class StateI : State
    {
        public override void Handle(FSMContext context, int decision)
        {
            return;
        }

        public override IReadOnlyCollection<Tuple<OS, int>> GetNextStates()
        {
            return new ReadOnlyCollection<Tuple<OS, int>>(new List<Tuple<OS, int>>());
        }
    }

    /* Odebrana */
    public class StateJ : StateI { }

}
