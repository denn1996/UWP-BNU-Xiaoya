﻿using CXHttpNS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xiaoya.Library.Seat.Models;

namespace Xiaoya.Library.Seat
{
    public class SeatClient
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        private CXSession m_Session = new CXSession();

        private string m_Token = "";

        private const string URL_LOGIN = "http://219.224.23.151/rest/auth?username={0}&password={1}";
        private const string URL_HISTORY = "http://219.224.23.151/rest/v2/history/{0}/{1}?token={2}";
        private const string URL_BUILDING = "http://219.224.23.151/rest/v2/free/filters?token={0}";
        private const string URL_ROOM = "http://219.224.23.151/rest/v2/room/stats2/{0}?token={1}";
        private const string URL_CURRENT_RESERVATION = "http://219.224.23.151/rest/v2/user/reservations/?token={0}";
        private const string URL_RESERVATION = "http://219.224.23.151/rest/view/{0}?token={1}";
        private const string URL_CANCEL_RESERVATION = "http://219.224.23.151/rest/v2/cancel/{0}?token={1}";
        private const string URL_LAYOUT = "http://219.224.23.151/rest/v2/room/layoutByDate/{0}/{1}?token={2}";
        private const string URL_START_TIME = "http://219.224.23.151/rest/v2/startTimesForSeat/{0}/{1}?token={2}";
        private const string URL_END_TIME = "http://219.224.23.151/rest/v2/endTimesForSeat/{0}/{1}/{2}?token={3}";
        private const string URL_ORDER = "http://219.224.23.151/rest/v2/freeBook";
        
        public SeatClient(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public async Task<Result<LoginToken>> Login()
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_LOGIN, Username, Password))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<LoginToken>>(body);

            m_Token = result.Data.Token;

            return result;
        }

        public async Task<Result<ReservationHistories>> GetReservationHistory(int start, int end)
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_HISTORY, start, end, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<ReservationHistories>>(body);

            return result;
        }

        public async Task<Result<BuildingInfo>> GetBuildings()
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_BUILDING, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<BuildingInfo>>(body);

            return result;
        }

        public async Task<Result<List<Room>>> GetRooms(int buildingId)
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_ROOM, buildingId, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<List<Room>>>(body);

            return result;
        }

        public async Task<Result<List<Reservation>>> GetCurrentReservation()
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_CURRENT_RESERVATION, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<List<Reservation>>>(body);

            return result;
        }

        public async Task<Reservation> GetReservation(int reservationId)
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_RESERVATION, reservationId, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Reservation>(body);

            return result;
        }

        public async Task<Result<string>> CancelReservation(int reservationId)
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_CANCEL_RESERVATION, reservationId, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<string>>(body);

            return result;
        }

        public async Task<Result<SeatLayout>> GetSeatLayout(int roomId, string date)
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_LAYOUT, roomId, date, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<SeatLayout>>(body);

            return result;
        }

        public async Task<Result<StartTimes>> GetStartTimes(int seatId, string date)
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_START_TIME, seatId, date, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<StartTimes>>(body);

            return result;
        }

        public async Task<Result<EndTimes>> GetEndTimes(int seatId, string date, string startTimeId)
        {
            var res = await m_Session.Req
                .Url(String.Format(URL_END_TIME, seatId, date, startTimeId, m_Token))
                .Get();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<EndTimes>>(body);

            return result;
        }

        public async Task<Result<Reservation>> OrderSeat(int seatId, string date, string startTimeId, string endTimeId)
        {
            var res = await m_Session.Req
                .Url(URL_ORDER)
                .Data("token", m_Token)
                .Data("startTime", startTimeId)
                .Data("endTime", endTimeId)
                .Data("seat", Convert.ToString(seatId))
                .Data("date", date)
                .Post();

            var body = await res.Content();

            var result = JsonConvert.DeserializeObject<Result<Reservation>>(body);

            return result;
        }
    }
}