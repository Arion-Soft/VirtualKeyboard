using System.Windows;
using System.Windows.Shell;

namespace VirtualKeyboard.Models{
    
    public class VJumpList{
        private readonly JumpList _mList;

        public string CategoryName { get; set; }
        public string AppPath { get; set; }

        public VJumpList(){
            _mList = new JumpList();
            JumpList.SetJumpList(Application.Current, _mList);

            CategoryName = "Category";
            AppPath = "";
        }

        public void Apply() { _mList.Apply(); }

        public void AddTask(string title,string desc, string arg){
            _mList.JumpItems.Add(new JumpTask() {
                Title           = title,
                Description     = desc,
                ApplicationPath = AppPath,
                Arguments       = arg,
                CustomCategory  = CategoryName
            });            
        }
    }
}
