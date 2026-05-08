namespace Formacio.Domain.Bodies;

public enum FichaMatriculaState { EmBranco, Preenchida }
public enum ContratoState { EmBranco, AssinadoPeloInteressado, AssinadoPorAmbos }
public enum ComprovanteState { NaoGerado, Gerado }
public enum PlanilhaFinanceiraState { EmUso }
public enum PlanilhaPresencaState { EmBranco, Preenchida, Arquivada }
public enum CertificadoState { EmBranco, Emitido, AssinadoPeloInstrutor, AssinadoPorAmbos, Entregue }
public enum ReciboState { EmBranco, Assinado }
