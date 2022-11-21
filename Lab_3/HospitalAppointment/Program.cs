static void Main() { }
public enum ReceptionHours { eight, nine, ten, eleven, twelve, fourteen, fifteen, sixteen, seventeen}
    public enum Doctors {dentist, surgeon, urologist, neurologist, otolaryngologist, psychiatrist, pediatrician, ophthalmologist, gynecologist};
    public interface IAppointment
    {
        public string GetName();
        public DateTime GetDate();
        public ReceptionHours GetTime();
        public Doctors GetDoctor();
    }
    public class Equeue
    {
        public bool AddAppointment(IAppointment client)
        {
            bool result = true;
            foreach (IAppointment appointment in this.allAppointments)
            {
                if ((appointment.GetDate == client.GetDate)
                   &(appointment.GetTime == client.GetTime)
                   &(appointment.GetDoctor == client.GetDoctor))
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                this.allAppointments.Add(client);
                this.allAppointmentsCount++;
            }
            return result;
        }

        public bool DeleteAppointment(IAppointment client)
        {
            return (this.allAppointments.Remove(client));
        }
        public List<IAppointment> GetAppointmentsByName(string clientName)
        {
            List<IAppointment> currentNameAppointments = new List<IAppointment>();
            string currentName;
            foreach (IAppointment appointment in this.allAppointments)
            {
                currentName = appointment.GetName();
                if (currentName == clientName)
                {
                    currentNameAppointments.Add(appointment);
                }
                // возможна сортировка
            }
            return currentNameAppointments;
        }
        public List<IAppointment> GetAppointmentsByDoctorAndDate(Doctors doctor, DateTime requiredDate)
        {
            List<IAppointment> currentDoctorAndDateAppointments = new List<IAppointment>();
            Doctors currentDoctor;
            DateTime currentDate;
            foreach (IAppointment appointment in this.allAppointments)
            {
                currentDoctor = appointment.GetDoctor();
                currentDate = appointment.GetDate();
                if ((currentDoctor == doctor) && (currentDate == requiredDate))
                {
                    currentDoctorAndDateAppointments.Add(appointment);
                }
                // возможна сортировка
            }
            return currentDoctorAndDateAppointments;
        }
        public List<IAppointment> GetAppointmentsByDate(DateTime requiredDate)
        {
            List<IAppointment> currentDateAppointments = new List<IAppointment>();
            DateTime currentDate;
            foreach (IAppointment appointment in this.allAppointments)
            {
                currentDate = appointment.GetDate();
                if (currentDate == requiredDate)
                {
                    currentDateAppointments.Add(appointment);
                }
                // возможна сортировка
            }
            return currentDateAppointments;
        }
        public List<ReceptionHours> GetFreeTimesByDoctorAndDate(Doctors doctor, DateTime requiredDate)
        {
            List<IAppointment> currentDateAppointments = GetAppointmentsByDoctorAndDate(doctor, requiredDate);
            List<ReceptionHours> freeTimes = new List<ReceptionHours>();
            ReceptionHours workTime;
            for (workTime = ReceptionHours.eight; workTime <= ReceptionHours.seventeen; workTime++)
            {
                freeTimes.Add(workTime);
            }
            foreach (IAppointment appointment in currentDateAppointments)
            {
                workTime = appointment.GetTime();
                if (freeTimes.Contains(workTime))
                {
                    freeTimes.Remove(workTime);
                }
                // возможна сортировка
            }
            return freeTimes;
        }
    void Main()
    {
        Console.WriteLine("Hey");
    }

    private List<IAppointment> allAppointments = new List<IAppointment>();
        private int allAppointmentsCount = 0;
    }