using System;
using System.Collections.Generic;
using KillPythonProcessDemo.Models;
using KillPythonProcessDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace KillPythonProcessDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _scriptToRun;
        private readonly IProcessManagementService _processManagementService;
        private const string PROCESSES = "_processes";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public HomeController(IOptions<GeneralSettingsModel> generalSettings, IProcessManagementService processManagementService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _processManagementService = processManagementService;
            _scriptToRun = generalSettings.Value.ScriptToRun;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewProcess()
        {
            List<ProcessModel> processes = new List<ProcessModel>();
            try
            {
                processes = GetProcessesFromSession();
            } catch {

            }

            var sysId = _processManagementService.StartNewProcess(_scriptToRun);
            processes.Add(new ProcessModel
            {
                SystemProcessId = sysId,
                FileName = _scriptToRun
            });
            SetProcessesToSession(processes);

            return View("Index", processes);
        }

        [HttpPost]
        public IActionResult KillProcess(int id)
        {
            var processes = GetProcessesFromSession();
            _processManagementService.KillProcess(id);
            var process = processes.Find(p => p.SystemProcessId == id);
            processes.Remove(process);
            SetProcessesToSession(processes);

            return View("Index", processes);
        }

        private List<ProcessModel> GetProcessesFromSession()
        {
            return JsonConvert.DeserializeObject<List<ProcessModel>>(_session.GetString(PROCESSES));
        }

        private void SetProcessesToSession(List<ProcessModel> processes)
        {
            _session.SetString(PROCESSES, JsonConvert.SerializeObject(processes));
        }
    }
}
