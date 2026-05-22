-- Script SQL inicial para Inbox, Histórico de Processamento e Outbox

-- Tabela: ProcessedMessages (Inbox)
CREATE TABLE IF NOT EXISTS ProcessedMessages (
    messageId NVARCHAR(128) PRIMARY KEY,
    firstProcessedAtUtc DATETIME2 NOT NULL,
    lastStatus NVARCHAR(32) NOT NULL
);

-- Tabela: RegistroProcessamento (Histórico)
CREATE TABLE IF NOT EXISTS RegistroProcessamento (
    processingId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    messageId NVARCHAR(128) NOT NULL,
    correlationId NVARCHAR(128) NOT NULL,
    status NVARCHAR(32) NOT NULL,
    attempt INT NOT NULL,
    failureReason NVARCHAR(512) NULL,
    processedAtUtc DATETIME2 NOT NULL,
    CONSTRAINT IX_RegistroProcessamento_MessageId_CorrelationId UNIQUE (messageId, correlationId, attempt)
);

-- Tabela: Outbox
CREATE TABLE IF NOT EXISTS Outbox (
    outboxId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    messageId NVARCHAR(128) NOT NULL,
    payload NVARCHAR(MAX) NOT NULL,
    publishedAtUtc DATETIME2 NULL,
    status NVARCHAR(32) NOT NULL
);
