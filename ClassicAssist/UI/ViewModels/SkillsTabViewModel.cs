using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Assistant;
using ClassicAssist.Data.Skills;
using ClassicAssist.UI.Misc;
using ClassicAssist.UO;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network;

namespace ClassicAssist.UI.ViewModels
{
    public class SkillsTabViewModel : BaseViewModel
    {
        private ObservableCollectionEx<SkillEntry> _items = new ObservableCollectionEx<SkillEntry>();
        private SkillEntry[] _selectedItems;
        private SkillEntry _selectedSkillEntry;
        private float _totalBase;
        private readonly Dispatcher _dispatcher;
        private ICommand _setAllSkillLocksCommand;

        public ICommand SetAllSkillLocksCommand => _setAllSkillLocksCommand ?? (_setAllSkillLocksCommand = new RelayCommand(SetAllSkillLocks, o => true));

        private void SetAllSkillLocks( object obj )
        {
            LockStatus lockStatus = (LockStatus)(int)obj;

            IEnumerable<SkillEntry> skillsToSet = Items.Where(i => i.LockStatus != lockStatus);

            foreach ( SkillEntry skillEntry in skillsToSet )
            {
                Commands.ChangeSkillLock( skillEntry, lockStatus );
            }

            Commands.MobileQuery(Engine.Player.Serial, Commands.MobileQueryType.SkillsRequest );
        }

        public SkillsTabViewModel()
        {
            Items.CollectionChanged += (sender, args) => { UpdateTotalBase(); };

            IncomingPacketHandlers.SkillUpdatedEvent += OnSkillUpdatedEvent;
            IncomingPacketHandlers.SkillsListEvent += OnSkillsListEvent;

            if ( Engine.Player != null )
            {
                Commands.MobileQuery( Engine.Player.Serial, Commands.MobileQueryType.SkillsRequest );
            }

            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void UpdateTotalBase()
        {
            TotalBase = Items.Sum(se => se.Base);
        }

        public ObservableCollectionEx<SkillEntry> Items
        {
            get => _items;
            set => SetProperty( ref _items, value );
        }

        public SkillEntry[] SelectedItems
        {
            get => _selectedItems;
            set => SetProperty( ref _selectedItems, value );
        }

        public SkillEntry SelectedSkillEntry
        {
            get => _selectedSkillEntry;
            set => SetProperty( ref _selectedSkillEntry, value );
        }

        public float TotalBase
        {
            get => _totalBase;
            set => SetProperty( ref _totalBase, value );
        }

        private void OnSkillsListEvent( SkillInfo[] skills )
        {
            _dispatcher.Invoke( () => { Items.Clear(); } );

            foreach ( SkillInfo si in skills )
            {
                Skill skill = new Skill { ID = si.ID, Name = Skills.GetSkillName( si.ID ) };

                SkillEntry se = new SkillEntry
                {
                    Skill = skill,
                    Value = si.Value,
                    Base = si.BaseValue,
                    Cap = si.SkillCap,
                    LockStatus = si.LockStatus
                };

                _dispatcher.Invoke( () => { Items.Add( se ); } );
            }
        }

        private void OnSkillUpdatedEvent( int skillID, float value, float baseValue, LockStatus lockStatus,
            float skillCap )
        {
            SkillEntry entry = _items.FirstOrDefault( se => se.Skill.ID == skillID );

            if ( entry == null )
            {
                return;
            }

            _dispatcher.Invoke( () =>
            {
                entry.Delta += baseValue - entry.Base;
                entry.Value = value;
                entry.Base = baseValue;
                entry.Cap = skillCap;
                entry.LockStatus = lockStatus;
            } );
        }
    }
}