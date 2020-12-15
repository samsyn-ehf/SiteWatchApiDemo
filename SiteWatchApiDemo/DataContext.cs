using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SiteWatchApiDemo
{
    public class DataContext : NotifyPropertyChanged
    {
        private readonly SiteWatchWebApi.SiteWatchApi siteWatchApi;
        private bool isLogedIn = false;
        private string username = "";
        private string password = "";
        private string search;
        private SiteWatchWebApiModel.Vehicle.ShowVehicleResult showVehicleResult;
        private SiteWatchWebApiModel.Search.SearchResult searchResult;

        private string avlJobName = "SiteWatch Demo";
        private string avlJobRemarks = "Demo";
        private DateTime avlJobStart = DateTime.Now;
        private DateTime avlJobEnd = DateTime.Now.AddHours(4);
        public string avlJobRefTable = string.Empty;
        public string avlJobRefId = null; // string.Empty;
        public string avlJobResult = string.Empty;

        private List<SiteWatchWebApiModel.AvlJob.AvlJobJobInsertStop> avlJobStops = new List<SiteWatchWebApiModel.AvlJob.AvlJobJobInsertStop>()
        {
                            new SiteWatchWebApiModel.AvlJob.AvlJobJobInsertStop
                            {
                                Text = "Samsýn",
                                DLat = 64.1323,
                                DLon = -21.8835,
                                PLat = 64.1323,
                                PLon = -21.8835,
                                Order = 1,
                            }
        };

        private CancellationTokenSource cancellationTokenSourceSearch;
        public DateTime Now { get { return DateTime.Now;  } }
        public bool IsLogedIn { get { return this.isLogedIn; } private set { this.SetField(ref this.isLogedIn, value); } }
        public string UserName { get { return this.username; } set { this.SetField(ref this.username, value); } }
        public string Password { get { return this.password; } set { this.SetField(ref this.password, value); } }
        public string Search { 
            get { return this.search; } 
            set { 
                if (this.SetField(ref this.search, value)) {
                    this.SiteWatchSearch(value);
                } 
            } 
        }

        public string AvlJobName { get { return this.avlJobName; } set { this.SetField(ref this.avlJobName, value); } }
        public string AvlJobRemarks { get { return this.avlJobRemarks; } set { this.SetField(ref this.avlJobRemarks, value); } }
        public DateTime AvlJobStart { get { return this.avlJobStart; } set { this.SetField(ref this.avlJobStart, value); } }
        public DateTime AvlJobEnd { get { return this.avlJobEnd; } set { this.SetField(ref this.avlJobEnd, value); } }
        public string AvlJobRefTable { get { return this.avlJobRefTable; } set { this.SetField(ref this.avlJobRefTable, value); } }
        public string AvlJobRefId { get { return this.avlJobRefId; } set { this.SetField(ref this.avlJobRefId, value); } }
        public string AvlJobResult { get { return this.avlJobResult; } set { this.SetField(ref this.avlJobResult, value); } }

        public List<SiteWatchWebApiModel.AvlJob.AvlJobJobInsertStop> AvlJobStops { get { return this.avlJobStops; } set { this.SetField(ref this.avlJobStops, value); } }

        public SiteWatchWebApiModel.Vehicle.ShowVehicleResult ShowVehicleResult { get { return this.showVehicleResult; } private set { this.SetField(ref this.showVehicleResult, value); } }
        public SiteWatchWebApiModel.Search.SearchResult SearchResult { get { return this.searchResult; } private set { this.SetField(ref this.searchResult, value); } }
        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand NewAvlJobCommand { get; private set; }
        public DataContext()
        {
            this.siteWatchApi = new SiteWatchWebApi.SiteWatchApi();
            this.LoginCommand = new RelayCommand(async (_) => { await this.LoginAsync(this.username, this.password); });
            this.NewAvlJobCommand = new RelayCommand(async (_) => { await this.NewAvlJob(); });
        }

        public async Task LoginAsync(string username, string password)
        {
            try
            {
                await this.siteWatchApi.LoginAsync(username, password);
                this.IsLogedIn = this.siteWatchApi.IsLogedIn;
                var input = new SiteWatchWebApiModel.Vehicle.ShowVehicleInput();
                this.ShowVehicleResult = await this.siteWatchApi.ShowVehiclesAsync(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                this.isLogedIn = false;
            }
        }

        public async Task NewAvlJob()
        {
            try
            {
                var jobInsert = new SiteWatchWebApiModel.AvlJob.AvlJobJobInsert
                {
                    Start = this.AvlJobStart,
                    End = this.AvlJobEnd,
                    Name = this.AvlJobName,
                    Remarks = this.AvlJobRemarks,
                    RefTable = this.AvlJobRefTable,
                    RefId = this.AvlJobRefId,
                    Stops = this.avlJobStops.ToArray()
                };

                var input = new SiteWatchWebApiModel.AvlJob.AvlJobRequest
                {
                    JobInsert = new[] { jobInsert }
                };

                var result = await this.siteWatchApi.AvlJobBatchUpdate(input);
                this.AvlJobResult = "OK";
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                this.AvlJobResult = ex.ToString();
                Console.WriteLine(ex);
            }
        }

        private async void SiteWatchSearch(string value)
        {
            try
            {
                if (this.cancellationTokenSourceSearch != null)
                {
                    this.cancellationTokenSourceSearch.Cancel();
                }

                this.cancellationTokenSourceSearch = new CancellationTokenSource();
                var input = new SiteWatchWebApiModel.Search.SearchInput()
                {
                    Search = value
                };

                this.SearchResult = await this.siteWatchApi.MapSearchAsync(input, this.cancellationTokenSourceSearch.Token);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
