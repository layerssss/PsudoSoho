﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.239
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MFLAdmin.LodgeService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LodgeService.WebServiceSoap")]
    public interface WebServiceSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 hashStr 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DeleteFolder", ReplyAction="*")]
        MFLAdmin.LodgeService.DeleteFolderResponse DeleteFolder(MFLAdmin.LodgeService.DeleteFolderRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 hashStr 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DeleteFile", ReplyAction="*")]
        MFLAdmin.LodgeService.DeleteFileResponse DeleteFile(MFLAdmin.LodgeService.DeleteFileRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 hashStr 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/EmptyFolder", ReplyAction="*")]
        MFLAdmin.LodgeService.EmptyFolderResponse EmptyFolder(MFLAdmin.LodgeService.EmptyFolderRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 hashStr 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WriteFile", ReplyAction="*")]
        MFLAdmin.LodgeService.WriteFileResponse WriteFile(MFLAdmin.LodgeService.WriteFileRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteFolderRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteFolder", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.DeleteFolderRequestBody Body;
        
        public DeleteFolderRequest() {
        }
        
        public DeleteFolderRequest(MFLAdmin.LodgeService.DeleteFolderRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class DeleteFolderRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string hashStr;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string path;
        
        public DeleteFolderRequestBody() {
        }
        
        public DeleteFolderRequestBody(string hashStr, string path) {
            this.hashStr = hashStr;
            this.path = path;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteFolderResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteFolderResponse", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.DeleteFolderResponseBody Body;
        
        public DeleteFolderResponse() {
        }
        
        public DeleteFolderResponse(MFLAdmin.LodgeService.DeleteFolderResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class DeleteFolderResponseBody {
        
        public DeleteFolderResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteFile", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.DeleteFileRequestBody Body;
        
        public DeleteFileRequest() {
        }
        
        public DeleteFileRequest(MFLAdmin.LodgeService.DeleteFileRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class DeleteFileRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string hashStr;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string path;
        
        public DeleteFileRequestBody() {
        }
        
        public DeleteFileRequestBody(string hashStr, string path) {
            this.hashStr = hashStr;
            this.path = path;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteFileResponse", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.DeleteFileResponseBody Body;
        
        public DeleteFileResponse() {
        }
        
        public DeleteFileResponse(MFLAdmin.LodgeService.DeleteFileResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class DeleteFileResponseBody {
        
        public DeleteFileResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EmptyFolderRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EmptyFolder", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.EmptyFolderRequestBody Body;
        
        public EmptyFolderRequest() {
        }
        
        public EmptyFolderRequest(MFLAdmin.LodgeService.EmptyFolderRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class EmptyFolderRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string hashStr;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string path;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pattern;
        
        public EmptyFolderRequestBody() {
        }
        
        public EmptyFolderRequestBody(string hashStr, string path, string pattern) {
            this.hashStr = hashStr;
            this.path = path;
            this.pattern = pattern;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EmptyFolderResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EmptyFolderResponse", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.EmptyFolderResponseBody Body;
        
        public EmptyFolderResponse() {
        }
        
        public EmptyFolderResponse(MFLAdmin.LodgeService.EmptyFolderResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class EmptyFolderResponseBody {
        
        public EmptyFolderResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class WriteFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="WriteFile", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.WriteFileRequestBody Body;
        
        public WriteFileRequest() {
        }
        
        public WriteFileRequest(MFLAdmin.LodgeService.WriteFileRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class WriteFileRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string hashStr;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string path;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string content;
        
        public WriteFileRequestBody() {
        }
        
        public WriteFileRequestBody(string hashStr, string path, string content) {
            this.hashStr = hashStr;
            this.path = path;
            this.content = content;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class WriteFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="WriteFileResponse", Namespace="http://tempuri.org/", Order=0)]
        public MFLAdmin.LodgeService.WriteFileResponseBody Body;
        
        public WriteFileResponse() {
        }
        
        public WriteFileResponse(MFLAdmin.LodgeService.WriteFileResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class WriteFileResponseBody {
        
        public WriteFileResponseBody() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WebServiceSoapChannel : MFLAdmin.LodgeService.WebServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebServiceSoapClient : System.ServiceModel.ClientBase<MFLAdmin.LodgeService.WebServiceSoap>, MFLAdmin.LodgeService.WebServiceSoap {
        
        public WebServiceSoapClient() {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MFLAdmin.LodgeService.DeleteFolderResponse MFLAdmin.LodgeService.WebServiceSoap.DeleteFolder(MFLAdmin.LodgeService.DeleteFolderRequest request) {
            return base.Channel.DeleteFolder(request);
        }
        
        public void DeleteFolder(string hashStr, string path) {
            MFLAdmin.LodgeService.DeleteFolderRequest inValue = new MFLAdmin.LodgeService.DeleteFolderRequest();
            inValue.Body = new MFLAdmin.LodgeService.DeleteFolderRequestBody();
            inValue.Body.hashStr = hashStr;
            inValue.Body.path = path;
            MFLAdmin.LodgeService.DeleteFolderResponse retVal = ((MFLAdmin.LodgeService.WebServiceSoap)(this)).DeleteFolder(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MFLAdmin.LodgeService.DeleteFileResponse MFLAdmin.LodgeService.WebServiceSoap.DeleteFile(MFLAdmin.LodgeService.DeleteFileRequest request) {
            return base.Channel.DeleteFile(request);
        }
        
        public void DeleteFile(string hashStr, string path) {
            MFLAdmin.LodgeService.DeleteFileRequest inValue = new MFLAdmin.LodgeService.DeleteFileRequest();
            inValue.Body = new MFLAdmin.LodgeService.DeleteFileRequestBody();
            inValue.Body.hashStr = hashStr;
            inValue.Body.path = path;
            MFLAdmin.LodgeService.DeleteFileResponse retVal = ((MFLAdmin.LodgeService.WebServiceSoap)(this)).DeleteFile(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MFLAdmin.LodgeService.EmptyFolderResponse MFLAdmin.LodgeService.WebServiceSoap.EmptyFolder(MFLAdmin.LodgeService.EmptyFolderRequest request) {
            return base.Channel.EmptyFolder(request);
        }
        
        public void EmptyFolder(string hashStr, string path, string pattern) {
            MFLAdmin.LodgeService.EmptyFolderRequest inValue = new MFLAdmin.LodgeService.EmptyFolderRequest();
            inValue.Body = new MFLAdmin.LodgeService.EmptyFolderRequestBody();
            inValue.Body.hashStr = hashStr;
            inValue.Body.path = path;
            inValue.Body.pattern = pattern;
            MFLAdmin.LodgeService.EmptyFolderResponse retVal = ((MFLAdmin.LodgeService.WebServiceSoap)(this)).EmptyFolder(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MFLAdmin.LodgeService.WriteFileResponse MFLAdmin.LodgeService.WebServiceSoap.WriteFile(MFLAdmin.LodgeService.WriteFileRequest request) {
            return base.Channel.WriteFile(request);
        }
        
        public void WriteFile(string hashStr, string path, string content) {
            MFLAdmin.LodgeService.WriteFileRequest inValue = new MFLAdmin.LodgeService.WriteFileRequest();
            inValue.Body = new MFLAdmin.LodgeService.WriteFileRequestBody();
            inValue.Body.hashStr = hashStr;
            inValue.Body.path = path;
            inValue.Body.content = content;
            MFLAdmin.LodgeService.WriteFileResponse retVal = ((MFLAdmin.LodgeService.WebServiceSoap)(this)).WriteFile(inValue);
        }
    }
}
