using Newtonsoft.Json;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.Collections.ObjectModel;
using System.IO;
using WinCopies.Util;

namespace PluginWorkflows
{
  /// <summary>
  /// The Workflows plugin
  /// </summary>
  public class WorkflowsPlugin : Plugin
  {

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluginName { get; set; } = "Workflows";

    private static Settings pluginSettings = new();
    internal static Settings PluginSettings { get => pluginSettings; set => pluginSettings = value; }


    /// <summary>
    /// Loads plugin settings
    /// </summary>
    public WorkflowsPlugin()
    {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\PluginWorkflows\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName))!;
    }


    private static Collection<ListItem> CreateItems(string query)
    {
      return new Collection<ListItem>(
        FuzzySearch.SearchAll(query, new Collection<string>(
        PluginSettings.Workflows.Select(x => x[0]).ToList()), PluginSettings.FuzzySearchThreshold)
        // After getting the top results, make them ListItems
        .Select(x => (ListItem)new Workflow(PluginSettings.Workflows[x.Index][0], PluginSettings.Workflows[x.Index].Skip(1).ToList())).ToList()
      );
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Provides all workflow special commands on the AllWorkflowsCommand.<br />
    /// Provides the workflow on the workflow's command (first string in it's list)
    /// </summary>
    /// <param name="command"><inheritdoc/></param>
    /// <returns>the relevant workflow special commands</returns>
    public override Collection<ListItem> OnSpecialCommand(string command)
    {
      switch (command)
      {
        case var value when value == PluginSettings.AllWorkflowsCommand:
          {
            Collection<ListItem> items = new();
            foreach (List<string> workflow in PluginSettings.Workflows)
            {
              items.Add(new Workflow(workflow[0], workflow.Skip(1).ToList()));
            }
            return items;
          }
        default:
          {
            Collection<ListItem> items = new();
            foreach (List<string> workflow in PluginSettings.Workflows)
            {
              if (workflow[0] == command)
              {
                items.Add(new Workflow(workflow[0], workflow.Skip(1).ToList()));
                return items;
              }
            }
            return items;
          }
      }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>The AllWorkflowsCommand and the first string in every list in the Workflows setting</returns>
    public override Collection<String> SpecialCommands()
    {
      Collection<String> Commands = new() { PluginSettings.AllWorkflowsCommand };
      foreach (List<string> workflow in PluginSettings.Workflows)
      {
        Commands.Add(workflow[0]);
      }
      return Commands;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns></returns>
    public override Collection<ListItem> OnQueryChange(string query)
    {
      return CreateItems(query);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>
    /// The Signifier from plugin settings
    /// </returns>
    public override Collection<string> CommandSignifiers()
    {
      return new Collection<string>() { PluginSettings.Signifier };
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="command">The Signifier (Since there is only 1 signifier for this plugin), followed by the query being searched for</param>
    /// <returns>A search item to open the search in the user's browser</returns>
    public override Collection<ListItem> OnSignifier(string command)
    {
      command ??= "";
      command = command.Substring(PluginSettings.Signifier.Length);
      return FuzzySearch.Sort(command, CreateItems(command));
    }

  }

}
