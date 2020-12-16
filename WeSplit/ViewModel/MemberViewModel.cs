using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class MemberViewModel : BaseViewModel
    {
        private Member newMember;
        public Member NewMember
        {
            get
            {
                return newMember;
            }
            set
            {
                OnPropertyChanged(ref newMember, value);
            }
        }
        private ObservableCollection<Member> members;
        public ObservableCollection<Member> Members
        {
            get
            {
                return members;
            }
            set
            {
                OnPropertyChanged(ref members, value);
            }
        }
        public ICommand AddMemberCommand { get; set; }
        public ICommand ResetViewCommand { get; set; }
        public ICommand SaveMembersCommand { get; set; }

        public MemberViewModel()
        {
            AddMemberCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (NewMember.IsAnyFieldNull())
                {
                    CustomDialog.ShowDialog("Có thông tin bỏ trống !", CustomDialog.Buttons.OK);
                    return;
                }
                if(!NewMember.PhoneNumber.All(char.IsDigit))
                {
                    CustomDialog.ShowDialog("Số điện thoại không đúng định dạng !", CustomDialog.Buttons.OK);
                    return;
                }    
                // Những thành viên đã có trong DB
                var existingLocations = new HashSet<string>(from member in Members select member.Name);
                // Kiểm tra địa điểm được thêm vào có trong DB chưa
                bool isExisted = existingLocations.Any(memberName => memberName == NewMember.Name);
                if (!isExisted)
                {
                    // Thêm vào địa điểm mới lên UI
                    Members.Add(NewMember.Clone());
                    NewMember = new Member();
                    return;
                }
                NewMember = new Member();
                CustomDialog.ShowDialog("Địa điểm đã có !", CustomDialog.Buttons.OK);
            });
            ResetViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ResetView();
            });
            SaveMembersCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DataAccess.UpdateAddMembers(Members.ToList());
            });
            ResetView();
        }
        private void ResetView()
        {
            NewMember = new Member();
            Members = new ObservableCollection<Member>(DataAccess.GetAllMembers());
        }
    }
}
