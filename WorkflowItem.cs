using Quokka;
using Quokka.ListItems;
using System.Windows.Media.Imaging;

namespace Plugin_Workflows {
  class Workflow : ListItem {

    List<string> flow;

    public Workflow(string command, List<string> flow) {
      this.flow = flow;
      this.Name = command;
      this.Description = $"The \"{command}\" workflow";
      this.Icon = new BitmapImage(new Uri(
          Environment.CurrentDirectory + "\\PlugBoard\\Plugin_Workflows\\Plugin\\chains.png"));
    }

    public override void Execute() {
      foreach (string command in this.flow) {
        Quokka.SearchWindow.ProduceItems(command)[0].Execute();
        //items (most likely) close the search window on execute
        App.Current.MainWindow = new SearchWindow();
        App.Current.MainWindow.Show();
      }
      //close final window that was launched
      App.Current.MainWindow.Close();
    }
  }
}
