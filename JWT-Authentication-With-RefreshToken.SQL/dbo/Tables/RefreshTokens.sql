CREATE TABLE [dbo].[RefreshTokens] (
    [RefreshTokenId]  INT           IDENTITY (1, 1) NOT NULL,
    [UserId]          INT           CONSTRAINT [DF_RefreshTokens_RefreshTokenId] DEFAULT ((-1)) NOT NULL,
    [RefreshToken]    VARCHAR (200) NOT NULL,
    [ExpireDateTime]  DATETIME      NOT NULL,
    [CreatedByIp]     VARCHAR (50)  NOT NULL,
    [RevokedDateTime] DATETIME      NULL,
    [RevokedByIp]     VARCHAR (50)  NULL,
    [RevokedUserId]   INT           NULL,
    [ReplacedByToken] VARCHAR (200) NULL,
    [RevokeReason]    VARCHAR (300) NULL,
    [IsExpired]       CHAR (1)      NULL,
    [IsActive]        CHAR (1)      CONSTRAINT [DF_RefreshTokens_IsActive] DEFAULT ('Y') NOT NULL,
    [IsRevoked]       CHAR (1)      NULL,
    [CreatedDateTime] DATETIME      CONSTRAINT [DF_RefreshTokens_CreatedDateTime] DEFAULT (getdate()) NOT NULL,
    [CreatedUserId]   INT           NOT NULL,
    [ModifiedUserId]  INT           NULL,
    [Modifiedtime]    DATETIME      NULL,
    CONSTRAINT [Pk_RefreshTokens_RefreshTokenId] PRIMARY KEY CLUSTERED ([RefreshTokenId] ASC),
    CONSTRAINT [FK_RefreshTokens_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserDetails] ([UserId]) ON DELETE CASCADE,
    CONSTRAINT [UNQ_RefreshTokens_RefreshToken] UNIQUE NONCLUSTERED ([RefreshToken] ASC)
);

