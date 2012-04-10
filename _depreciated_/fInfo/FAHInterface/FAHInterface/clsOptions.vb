'/*
' * Code converted to vb.net from Harlam357 code hosted on http://code.google.com/p/hfm-net/source/browse/#svn%2Ftrunk%2Fsrc%2FHFM.Client 
' * Edits made mtm
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General Public License
' * as published by the Free Software Foundation; version 2
' * of the License. See the included file GPLv2.TXT.
' * 
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' * 
' * You should have received a copy of the GNU General Public License
' * along with this program; if not, write to the Free Software
' * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
' */
Imports System.Collections.Generic
Imports Newtonsoft.Json.Linq
Imports System.Runtime.Serialization

Namespace Client
    <Serializable()>
    Public Class Options
        Inherits Dictionary(Of String, String)
        Implements ISerializable
        Implements IEquatable(Of Options)
        Public ReadOnly Property assignment_servers As String
            Get
                Return Me("assignment-servers").ToString.Replace(Chr(34), "").Replace("assignment-servers:", "").Trim
            End Get
        End Property
        Public ReadOnly Property capture_directory As String
            Get
                Return Me("capture-directory").ToString.Replace(Chr(34), "").Replace("capture-directory:", "").Trim
            End Get
        End Property
        Public ReadOnly Property capture_sockets As String
            Get
                Return Me("capture-sockets").ToString.Replace(Chr(34), "").Replace("capture-sockets:", "").Trim
            End Get
        End Property
        Public ReadOnly Property checkpoint As Int32
            Get
                Return CInt(Me("checkpoint").ToString.Replace(Chr(34), "").Replace("checkpoint:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property child As String
            Get
                Return Me("child").ToString.Replace(Chr(34), "").Replace("child:", "").Trim
            End Get
        End Property
        Public ReadOnly Property client_subtype As String
            Get
                Return Me("client-subtype").ToString.Replace(Chr(34), "").Replace("client-subtype:", "").Trim
            End Get
        End Property
        Public ReadOnly Property client_type As String
            Get
                Return Me("client-type").ToString.Replace(Chr(34), "").Replace("client-type:", "").Trim
            End Get
        End Property
        Public ReadOnly Property command_address As String
            Get
                Return Me("command-address").ToString.Replace(Chr(34), "").Replace("command-address:", "").Trim
            End Get
        End Property
        Public ReadOnly Property command_allow As String
            Get
                Return Me("command-allow").ToString.Replace(Chr(34), "").Replace("command-allow:", "").Trim
            End Get
        End Property
        Public ReadOnly Property command_allow_no_pass As String
            Get
                Return Me("command-allow-no-pass").ToString.Replace(Chr(34), "").Replace("command-allow-no-pass:", "").Trim
            End Get
        End Property
        Public ReadOnly Property command_deny As String
            Get
                Return Me("command-deny").ToString.Replace(Chr(34), "").Replace("command-deny:", "").Trim
            End Get
        End Property
        Public ReadOnly Property command_deny_no_pass As String
            Get
                Return Me("command-deny-no-pass").ToString.Replace(Chr(34), "").Replace("command-deny-no-pass:", "").Trim
            End Get
        End Property
        Public ReadOnly Property command_port As Int32
            Get
                Return CInt(Me("command-port").ToString.Replace(Chr(34), "").Replace("command-port:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property config_rotate As String
            Get
                Return Me("config-rotate").ToString.Replace(Chr(34), "").Replace("config-rotate:", "").Trim
            End Get
        End Property
        Public ReadOnly Property config_rotate_dir As String
            Get
                Return Me("config-rotate-dir").ToString.Replace(Chr(34), "").Replace("config-rotate-dir:", "").Trim
            End Get
        End Property
        Public ReadOnly Property config_rotate_max As String
            Get
                Return Me("config-rotate-max").ToString.Replace(Chr(34), "").Replace("config-rotate-max:", "").Trim
            End Get
        End Property
        Public ReadOnly Property core_dir As String
            Get
                Return Me("core-dir").ToString.Replace(Chr(34), "").Replace("core-dir:", "").Trim
            End Get
        End Property
        Public ReadOnly Property core_key As String
            Get
                Try
                    Return Me("core-key").ToString.Replace(Chr(34), "").Replace("core-key:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property core_prep As String
            Get
                Try
                    Return Me("core-prep").ToString.Replace(Chr(34), "").Replace("core-prep:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property core_priority As String
            Get
                Return Me("core-priority").ToString.Replace(Chr(34), "").Replace("core-priority:", "").Trim
            End Get
        End Property
        Public ReadOnly Property core_server As String
            Get
                Try
                    Return Me("core-server").ToString.Replace(Chr(34), "").Replace("core-server:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property cpu_affinity As String
            Get
                Return Me("cpu-affinity").ToString.Replace(Chr(34), "").Replace("cpu-affinity:", "").Trim
            End Get
        End Property
        Public ReadOnly Property cpu_species As String
            Get
                Return Me("cpu-species").ToString.Replace(Chr(34), "").Replace("cpu-species:", "").Trim
            End Get
        End Property
        Public ReadOnly Property cpu_type As String
            Get
                Return Me("cpu-type").ToString.Replace(Chr(34), "").Replace("cpu-type:", "").Trim
            End Get
        End Property
        Public ReadOnly Property cpu_usage As Short
            Get
                Return CShort(Me("cpu-usage").ToString.Replace(Chr(34), "").Replace("cpu-usage:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property cpus As Short
            Get
                Return CShort(Me("cpus").ToString.Replace(Chr(34), "").Replace("cpus:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property cycle_rate As String
            Get
                Return Me("cycle-rate").ToString.Replace(Chr(34), "").Replace("cycle-rate:", "").Trim
            End Get
        End Property
        Public ReadOnly Property cycles As String
            Get
                Return Me("cycles").ToString.Replace(Chr(34), "").Replace("cycles:", "").Trim
            End Get
        End Property
        Public ReadOnly Property daemon As String
            Get
                Return Me("daemon").ToString.Replace(Chr(34), "").Replace("daemon:", "").Trim
            End Get
        End Property
        Public ReadOnly Property data_directory As String
            Get
                Return Me("data-directory").ToString.Replace(Chr(34), "").Replace("data-directory:", "").Trim
            End Get
        End Property
        Public ReadOnly Property debug_sockets As String
            Get
                Return Me("debug-sockets").ToString.Replace(Chr(34), "").Replace("debug-sockets:", "").Trim
            End Get
        End Property
        Public Overloads ReadOnly Property dump_after_deadline As Boolean
            Get
                Return CBool(Me("dump-after-deadline").ToString.Replace(Chr(34), "").Replace("dump-after-deadline:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property eval
            Get
                Try
                    Return Me("eval").ToString.Replace(Chr(34), "").Replace("eval:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property exception_locations As String
            Get
                Return Me("exception-locations").ToString.Replace(Chr(34), "").Replace("exception-locations:", "").Trim
            End Get
        End Property
        Public ReadOnly Property exec_directory As String
            Get
                Return Me("exec-directory").ToString.Replace(Chr(34), "").Replace("exec-directory:", "").Trim
            End Get
        End Property
        Public ReadOnly Property exit_when_done As Boolean
            Get
                Return CBool(Me("exit-when-done").ToString.Replace(Chr(34), "").Replace("exit-when-done:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property extra_core_args As String
            Get
                Try
                    Return Me("extra-core-args").ToString.Replace(Chr(34), "").Replace("extra-core-args:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property gpu As String
            Get
                Return Me("gpu").ToString.Replace(Chr(34), "").Replace("gpu:", "").Trim
            End Get
        End Property
        Public ReadOnly Property gpu_assignment_servers As String
            Get
                Return Me("gpu-assignment-servers").ToString.Replace(Chr(34), "").Replace("gpu-assignment-servers:", "").Trim
            End Get
        End Property
        Public ReadOnly Property gpu_device_id As String
            Get
                Try
                    Return Me("gpu-device-id").ToString.Replace(Chr(34), "").Replace("gpu-device-id:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property gpu_id As String
            Get
                Try
                    Return Me("gpu-id").ToString.Replace(Chr(34), "").Replace("gpu-id:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property gpu_index As Short
            Get
                Try
                    Return CShort(Me("gpu-index").ToString.Replace(Chr(34), "").Replace("gpu-index:", "").Trim)
                Catch ex As KeyNotFoundException
                    Return 0
                End Try
            End Get
        End Property
        Public ReadOnly Property gpu_vendor_id As String
            Get
                Try
                    Return Me("gpu-vendor-id").ToString.Replace(Chr(34), "").Replace("gpu-vendor-id:", "").Trim
                Catch ex As KeyNotFoundException
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property log As String
            Get
                Return Me("log").ToString.Replace(Chr(34), "").Replace("log:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_color As String 'no importing namespaces, return string
            Get
                Return Me("log-color").ToString.Replace(Chr(34), "").Replace("log-color:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_crlf As String
            Get
                Return Me("log-crlf").ToString.Replace(Chr(34), "").Replace("log-crlf:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_date As Boolean
            Get
                Return CBool(Me("log-date").ToString.Replace(Chr(34), "").Replace("log-date:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property log_debug As String
            Get
                Return Me("log-debug").ToString.Replace(Chr(34), "").Replace("log-debug:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_domain As String
            Get
                Return Me("log-domain").ToString.Replace(Chr(34), "").Replace("log-domain:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_header As String
            Get
                Return Me("log-header").ToString.Replace(Chr(34), "").Replace("log-header:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_level As String
            Get
                Return Me("log-level").ToString.Replace(Chr(34), "").Replace("log-level:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_no_info_header As String
            Get
                Return Me("log-no-info-header").ToString.Replace(Chr(34), "").Replace("log-no-info-header:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_redirect As String
            Get
                Return Me("log-redirect").ToString.Replace(Chr(34), "").Replace("log-redirect:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_rotate As String
            Get
                Return Me("log-rotate").ToString.Replace(Chr(34), "").Replace("log-rotate:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_rotate_dir
            Get
                Return Me("log-rotate-dir").ToString.Replace(Chr(34), "").Replace("log-rotate-dir:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_rotate_max As String
            Get
                Return Me("log-rotate-max").ToString.Replace(Chr(34), "").Replace("log-rotate-max:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_short_level As String
            Get
                Return Me("log-short-level").ToString.Replace(Chr(34), "").Replace("log-short-level:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_simple_domains As String
            Get
                Return Me("log-simple-domains").ToString.Replace(Chr(34), "").Replace("log-simple-domains:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_thread_id As String
            Get
                Return Me("log-thread-id").ToString.Replace(Chr(34), "").Replace("log-thread-id:", "").Trim
            End Get
        End Property
        Public ReadOnly Property log_time As Boolean
            Get
                Return CBool(Me("log-time").ToString.Replace(Chr(34), "").Replace("log-time:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property log_to_screen As Boolean
            Get
                Return CBool(Me("log-to-screen").ToString.Replace(Chr(34), "").Replace("log-to-screen:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property log_truncate As String
            Get
                Return Me("log-truncate").ToString.Replace(Chr(34), "").Replace("log-truncate:", "").Trim
            End Get
        End Property
        Public ReadOnly Property machine_id As Short
            Get
                Return CShort(Me("machine-id").ToString.Replace(Chr(34), "").Replace("machine-id:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property max_delay As String
            Get
                Return Me("max-delay").ToString.Replace(Chr(34), "").Replace("max-delay:", "").Trim
            End Get
        End Property
        Public ReadOnly Property max_packet_size As String
            Get
                Return Me("max-packet-size").ToString.Replace(Chr(34), "").Replace("max-packet-size:", "").Trim
            End Get
        End Property
        Public ReadOnly Property max_queue As String
            Get
                Return Me("max-queue").ToString.Replace(Chr(34), "").Replace("max-queue:", "").Trim
            End Get
        End Property
        Public ReadOnly Property max_shutdown_wait As String
            Get
                Return Me("max-shutdown-wait").ToString.Replace(Chr(34), "").Replace("max-shutdown-wait:", "").Trim
            End Get
        End Property
        Public ReadOnly Property max_slot_errors As Short
            Get
                Return CShort(Me("max-slot-errors").ToString.Replace(Chr(34), "").Replace("max-slot-errors:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property max_unit_errors As Short
            Get
                Return Me("max-unit-errors").ToString.Replace(Chr(34), "").Replace("max-unit-errors:", "").Trim
            End Get
        End Property
        Public ReadOnly Property max_units As Short
            Get
                Return CShort(Me("max-units").ToString.Replace(Chr(34), "").Replace("max-units:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property min_delay As String
            Get
                Return Me("min-delay").ToString.Replace(Chr(34), "").Replace("min-delay:", "").Trim
            End Get
        End Property
        Public ReadOnly Property next_unit_percentage As Short
            Get
                Return CShort(Me("next-unit-percentage").ToString.Replace(Chr(34), "").Replace("next-unit-percentage:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property priority As String
            Get
                Return Me("priority").ToString.Replace(Chr(34), "").Replace("priority:", "").Trim
            End Get
        End Property
        Public ReadOnly Property no_assembly As Boolean
            Get
                Return Me("no-assembly").ToString.Replace(Chr(34), "").Replace("no-assembly:", "").Trim
            End Get
        End Property
        Public ReadOnly Property os_species
            Get
                Return Me("os-species").ToString.Replace(Chr(34), "").Replace("os-species:", "").Trim
            End Get
        End Property
        Public ReadOnly Property os_type As String
            Get
                Return Me("os-type").ToString.Replace(Chr(34), "").Replace("os-type:", "").Trim
            End Get
        End Property
        Public ReadOnly Property passkey
            Get
                Return Me("passkey").ToString.Replace(Chr(34), "").Replace("passkey:", "").Trim
            End Get
        End Property
        Public ReadOnly Property password As String
            Get
                Return Me("password").ToString.Replace(Chr(34), "").Replace("password:", "").Trim
            End Get
        End Property
        Public ReadOnly Property pause_on_battery As Boolean
            Get
                Return CBool(Me("pause-on-battery").ToString.Replace(Chr(34), "").Replace("pause-on-battery:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property pause_on_start As Boolean
            Get
                Return Me("pause-on-start").ToString.Replace(Chr(34), "").Replace("pause-on-start:", "").Trim
            End Get
        End Property
        Public ReadOnly Property pid As String
            Get
                Return Me("pid").ToString.Replace(Chr(34), "").Replace("pid:", "").Trim
            End Get
        End Property
        Public ReadOnly Property pid_file As String
            Get
                Return Me("pid-file").ToString.Replace(Chr(34), "").Replace("pid-file:", "").Trim
            End Get
        End Property
        Public ReadOnly Property project_key As String
            Get
                Return Me("project-key").ToString.Replace(Chr(34), "").Replace("project-key:", "").Trim
            End Get
        End Property
        Public ReadOnly Property respawn As String
            Get
                Return Me("respawn").ToString.Replace(Chr(34), "").Replace("respawn:", "").Trim
            End Get
        End Property
        Public ReadOnly Property service As Boolean
            Get
                Return CBool(Me("service").ToString.Replace(Chr(34), "").Replace("service:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property service_description As String
            Get
                Return Me("service-description").ToString.Replace(Chr(34), "").Replace("service-description:", "").Trim
            End Get
        End Property
        Public ReadOnly Property service_restart As Boolean
            Get
                Return Me("service-restart").ToString.Replace(Chr(34), "").Replace("service-restart:", "").Trim
            End Get
        End Property
        Public ReadOnly Property service_restart_delay As Int16
            Get
                Return CInt(Me("service-restart-delay").ToString.Replace(Chr(34), "").Replace("service-restart-delay:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property smp As Boolean
            Get
                Return CBool(Me("smp").ToString.Replace(Chr(34), "").Replace("smp:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property stack_traces As String
            Get
                Return Me("stack-traces").ToString.Replace(Chr(34), "").Replace("stack-traces:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Team As String
            Get
                Return Me("team").ToString.Replace(Chr(34), "").Replace("team:", "").Trim
            End Get
        End Property
        Public ReadOnly Property threads As Short
            Get
                Return CShort(Me("threads").ToString.Replace(Chr(34), "").Replace("threads:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property user
            Get
                Return Me("user").ToString.Replace(Chr(34), "").Replace("user:", "").Trim
            End Get
        End Property
        Public ReadOnly Property verbosity As Short
            Get
                Return CShort(Me("verbosity").ToString.Replace(Chr(34), "").Replace("verbosity:", "").Trim)
            End Get
        End Property

        Public Shared Function Parse(value As String) As Options
            Try
                Dim o = JObject.Parse(value)
                Dim options = New Options()
                'For Each prop As Object In o.Properties()
                '    options.Add(prop.Name, DirectCast(prop, String))
                'Next
                For Each prop As Object In o.Properties()
                    options.Add(prop.Name, DirectCast(prop.ToString, String))
                Next
                Return options
            Catch ex As KeyNotFoundException

            End Try
        End Function

        Public Function Equals1(other As Options) As Boolean Implements System.IEquatable(Of Options).Equals
            Try
                Dim bSame As Boolean = True
                bSame = Me.verbosity = other.verbosity
                If Not bSame Then Return False
                bSame = Me.user = other.user
                If Not bSame Then Return False
                bSame = Me.threads = other.threads
                If Not bSame Then Return False
                bSame = Me.Team = other.Team
                If Not bSame Then Return False
                bSame = Me.stack_traces = other.stack_traces
                If Not bSame Then Return False
                bSame = Me.smp = other.smp
                If Not bSame Then Return False
                bSame = Me.service_restart_delay = other.service_restart_delay
                If Not bSame Then Return False
                bSame = Me.service_restart = other.service_restart
                If Not bSame Then Return False
                bSame = Me.service_description = other.service_description
                If Not bSame Then Return False
                bSame = Me.service = other.service
                If Not bSame Then Return False
                bSame = Me.pause_on_start = other.pause_on_start
                If Not bSame Then Return False
                bSame = Me.pause_on_battery = other.pause_on_battery
                If Not bSame Then Return False
                bSame = Me.password = other.password
                If Not bSame Then Return False
                bSame = Me.passkey = other.passkey
                If Not bSame Then Return False
                bSame = Me.os_type = other.os_type
                If Not bSame Then Return False
                bSame = Me.os_species = other.os_species
                If Not bSame Then Return False
                bSame = Me.no_assembly = other.no_assembly
                If Not bSame Then Return False
                bSame = Me.next_unit_percentage = other.next_unit_percentage
                If Not bSame Then Return False
                bSame = Me.min_delay = other.min_delay
                If Not bSame Then Return False
                bSame = Me.max_units = other.max_units
                If Not bSame Then Return False
                bSame = Me.max_unit_errors = other.max_unit_errors
                If Not bSame Then Return False
                bSame = Me.max_slot_errors = other.max_slot_errors
                If Not bSame Then Return False
                bSame = Me.max_shutdown_wait = other.max_shutdown_wait
                If Not bSame Then Return False
                bSame = Me.max_queue = other.max_queue
                If Not bSame Then Return False
                bSame = Me.max_packet_size = other.max_packet_size
                If Not bSame Then Return False
                bSame = Me.max_delay = other.max_delay
                If Not bSame Then Return False
                bSame = Me.machine_id = other.machine_id
                If Not bSame Then Return False
                bSame = Me.log_truncate = other.log_truncate
                If Not bSame Then Return False
                bSame = Me.log_to_screen = other.log_to_screen
                If Not bSame Then Return False
                bSame = Me.log_time = other.log_time
                If Not bSame Then Return False
                bSame = Me.log_thread_id = other.log_thread_id
                If Not bSame Then Return False
                bSame = Me.log_simple_domains = other.log_simple_domains
                If Not bSame Then Return False
                bSame = Me.log_short_level = other.log_short_level
                If Not bSame Then Return False
                bSame = Me.log_rotate_max = other.log_rotate_max
                If Not bSame Then Return False
                bSame = Me.log_rotate_dir = other.log_rotate_dir
                If Not bSame Then Return False
                bSame = Me.log_rotate = other.log_rotate
                If Not bSame Then Return False
                bSame = Me.log_redirect = other.log_redirect
                If Not bSame Then Return False
                bSame = Me.log_no_info_header = other.log_no_info_header
                If Not bSame Then Return False
                bSame = Me.log_level = other.log_level
                If Not bSame Then Return False
                bSame = Me.log_header = other.log_header
                If Not bSame Then Return False
                bSame = Me.log_domain = other.log_domain
                If Not bSame Then Return False
                bSame = Me.log_debug = other.log_debug
                If Not bSame Then Return False
                bSame = Me.log_date = other.log_date
                If Not bSame Then Return False
                bSame = Me.log_crlf = other.log_crlf
                If Not bSame Then Return False
                bSame = Me.log_color = other.log_color
                If Not bSame Then Return False
                bSame = Me.log = other.log
                If Not bSame Then Return False
                bSame = Me.gpu_vendor_id = other.gpu_vendor_id
                If Not bSame Then Return False
                bSame = Me.gpu_index = other.gpu_index
                If Not bSame Then Return False
                bSame = Me.gpu_id = other.gpu_id
                If Not bSame Then Return False
                bSame = Me.gpu_device_id = other.gpu_device_id
                If Not bSame Then Return False
                bSame = Me.gpu_assignment_servers = other.gpu_assignment_servers
                If Not bSame Then Return False
                bSame = Me.gpu = other.gpu
                If Not bSame Then Return False
                bSame = Me.extra_core_args = other.extra_core_args
                If Not bSame Then Return False
                bSame = Me.exit_when_done = other.exit_when_done
                If Not bSame Then Return False
                bSame = Me.exec_directory = other.exec_directory
                If Not bSame Then Return False
                bSame = Me.exception_locations = other.exception_locations
                If Not bSame Then Return False
                bSame = Me.eval = other.eval
                If Not bSame Then Return False
                bSame = Me.dump_after_deadline = other.dump_after_deadline
                If Not bSame Then Return False
                bSame = Me.debug_sockets = other.debug_sockets
                If Not bSame Then Return False
                bSame = Me.data_directory = other.data_directory
                If Not bSame Then Return False
                bSame = Me.daemon = other.daemon
                If Not bSame Then Return False
                bSame = Me.cycles = other.cycles
                If Not bSame Then Return False
                bSame = Me.cycle_rate = other.cycle_rate
                If Not bSame Then Return False
                bSame = Me.cpus = other.cpus
                If Not bSame Then Return False
                bSame = Me.cpu_usage = other.cpu_usage
                If Not bSame Then Return False
                bSame = Me.cpu_type = other.cpu_type
                If Not bSame Then Return False
                bSame = Me.cpu_species = other.cpu_species
                If Not bSame Then Return False
                bSame = Me.cpu_affinity = other.cpu_affinity
                If Not bSame Then Return False
                bSame = Me.core_server = other.core_server
                If Not bSame Then Return False
                bSame = Me.core_priority = other.core_priority
                If Not bSame Then Return False
                bSame = Me.core_prep = other.core_prep
                If Not bSame Then Return False
                bSame = Me.core_key = other.core_key
                If Not bSame Then Return False
                bSame = Me.core_dir = other.core_dir
                If Not bSame Then Return False
                bSame = Me.config_rotate_max = other.config_rotate_max
                If Not bSame Then Return False
                bSame = Me.config_rotate_dir = other.config_rotate_dir
                If Not bSame Then Return False
                bSame = Me.config_rotate = other.config_rotate
                If Not bSame Then Return False
                bSame = Me.command_port = other.command_port
                If Not bSame Then Return False
                bSame = Me.command_deny_no_pass = other.command_deny_no_pass
                If Not bSame Then Return False
                bSame = Me.command_deny = other.command_deny
                If Not bSame Then Return False
                bSame = Me.command_allow_no_pass = other.command_allow_no_pass
                If Not bSame Then Return False
                bSame = Me.command_allow = other.command_allow
                If Not bSame Then Return False
                bSame = Me.command_address = other.command_address
                If Not bSame Then Return False
                bSame = Me.client_type = other.client_type
                If Not bSame Then Return False
                bSame = Me.client_subtype = other.client_subtype
                If Not bSame Then Return False
                bSame = Me.child = other.child
                If Not bSame Then Return False
                bSame = Me.checkpoint = other.checkpoint
                If Not bSame Then Return False
                bSame = Me.capture_sockets = other.capture_sockets
                If Not bSame Then Return False
                bSame = Me.capture_directory = other.capture_directory
                If Not bSame Then Return False
                bSame = Me.assignment_servers = other.assignment_servers
                Return bSame
            Catch ex As KeyNotFoundException

            End Try
        End Function
#Region "Log extender"
        Public Class cLW
            ' NOTE function names differ due to already declard Log
            Public Event Log(ByVal Message As String)
            Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
            Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
                RaiseEvent LogError(Message, EObj)
            End Sub
            Public Sub WriteLog(ByVal Message As String)
                RaiseEvent Log(Message)
            End Sub
        End Class
        Public Shared WithEvents LogWindow As New cLW
        Public Shared Event WriteLog(ByVal Message As String)
        Public Shared Event WriteError(ByVal Message As String, ByVal EObj As ErrObject)
        Private Shared Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
            RaiseEvent WriteLog(Message)
        End Sub
        Private Shared Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
            RaiseEvent WriteError(Message, EObj)
        End Sub
#End Region
    End Class
End Namespace
